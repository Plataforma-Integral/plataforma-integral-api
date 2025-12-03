using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using PlataformaIntegral.API.DTOs;
using PlataformaIntegral.API.Enums;
using PlataformaIntegral.API.Models;
using PlataformaIntegral.API.Services;
using Xabe.FFmpeg;

public class CrearCursoService : ICrearCursoService
{
    private readonly PlataformaIntegralContext _context;
    private readonly IMapper _mapper;
    private readonly MinioService _storage;

    public CrearCursoService(
        PlataformaIntegralContext context,
        IMapper mapper,
        MinioService storage)
    {
        _context = context;
        _mapper = mapper;
        _storage = storage;
    }

    // ========================================
    // 1. CREAR CURSO PREGRABADO
    // ========================================
    public async Task<int> CrearCursoPregrabadoAsync([FromForm] CursoPregrabadoDto dto)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var producto = new Producto
            {
                Nombre = dto.Titulo,
                Descripcion = dto.Descripcion,
                Precio = dto.Precio,
                FechaCreacion = DateTime.UtcNow
            };

            var curso = new Curso
            {
                IdProducto = producto.IdProducto,
                Estado = EstadoCursoEnum.Borrador,
                Producto = producto
            };

            var cursoPregrabado = new CursoPregrabado
            {
                IdCurso = curso.IdProducto,
                Curso = curso,
                PrecioPuntos = dto.PrecioPuntos,
                UrlPortada = dto.Portada != null
                    ? await _storage.UploadImageAsync(dto.Portada)
                    : null
            };

            _context.CursoPregrabados.Add(cursoPregrabado);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return cursoPregrabado.IdCurso;
        }
        catch
        {
            if (transaction.GetDbTransaction().Connection != null)
            {
                await transaction.RollbackAsync();
            }
            throw;
        }
    }

    // ========================================
    // 2. AGREGAR CAPÍTULO
    // ========================================
    public async Task<int> AgregarCapituloAsync(int cursoPregrabadoId, string nombreCapitulo)
    {
        var numero = await _context.Capitulos
            .CountAsync(c => c.IdCursoPregrabado == cursoPregrabadoId) + 1;

        var capitulo = new Capitulo
        {
            IdCursoPregrabado = cursoPregrabadoId,
            Nombre = nombreCapitulo,
            NumeroOrden = numero
        };

        _context.Capitulos.Add(capitulo);
        await _context.SaveChangesAsync();

        return capitulo.IdCapitulo;
    }

    // ========================================
    // 3. SUBIR VIDEO
    // ========================================
    public async Task<int> SubirVideoAsync(int capituloId, [FromForm] VideoDto dto)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            // 1) Subir video a MinIO
            string videoKey = await _storage.UploadVideoAsync(dto.Archivo);

            // Guardar temporalmente para procesar con FFmpeg
            var tempVideoPath = Path.Combine(Path.GetTempPath(), dto.Archivo.FileName);
            using (var stream = new FileStream(tempVideoPath, FileMode.Create))
            {
                await dto.Archivo.CopyToAsync(stream);
            }


            // 2) Obtener duración y peso
            var mediaInfo = await FFmpeg.GetMediaInfo(tempVideoPath);
            var duracion = (int)mediaInfo.Duration.TotalSeconds;
            var peso = new FileInfo(tempVideoPath).Length;

            // 3) Miniatura
            string miniaturaKey;
            if (dto.Miniatura != null) // ojo: usa PascalCase en la propiedad
            {
                miniaturaKey = await _storage.UploadMiniaturaAsync(dto.Miniatura);
            }
            else
            {
                var tempThumbPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.jpg");
                var conversion = FFmpeg.Conversions.New()
                    .AddParameter($"-ss {duracion / 2} -i {tempVideoPath} -frames:v 1 {tempThumbPath}");
                await conversion.Start();

                using var thumbStream = new FileStream(tempThumbPath, FileMode.Open, FileAccess.Read);

                // Aquí construyes el FormFile con ContentType y Headers válidos
                var formFile = new FormFile(thumbStream, 0, thumbStream.Length, "miniatura", Path.GetFileName(tempThumbPath))
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "image/jpeg"
                };

                miniaturaKey = await _storage.UploadMiniaturaAsync(formFile);
            }

            // 4) Crear entidad Recurso
            var recurso = new Recurso
            {
                Nombre = dto.Nombre,
                Url = videoKey,
            };

            _context.Recursos.Add(recurso);
            await _context.SaveChangesAsync();

            // 5) Crear entidad Video
            var numeroOrden = await _context.Videos
                .CountAsync(v => v.IdCapitulo == capituloId) + 1;

            var video = new Video
            {
                IdCapitulo = capituloId,
                IdRecurso = recurso.IdRecurso,
                NumeroOrden = numeroOrden,
                MiniaturaUrl = miniaturaKey,
                DuracionSegundos = duracion,
                PesoBytes = peso
            };

            _context.Videos.Add(video);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
            return recurso.IdRecurso;
        }
        catch
        {
            if (transaction.GetDbTransaction().Connection != null)
            {
                await transaction.RollbackAsync();
            }
            throw;
        }
    }

    // ========================================
    // 4. PUBLICAR CURSO
    // ========================================
    public async Task PublicarCursoAsync(int cursoId)
    {
        var curso = await _context.Cursos.FindAsync(cursoId);

        if (curso == null)
            throw new Exception($"Curso {cursoId} no encontrado.");

        curso.Estado = EstadoCursoEnum.Publicado;

        await _context.SaveChangesAsync();
    }

    public async Task ModificarCursoPregrabadoAsync(int cursoId, CursoPregrabadoDto dto)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var curso = await _context.Cursos
                .Include(c => c.Producto)
                .Include(c => c.CursoPregrabado)
                .FirstOrDefaultAsync(c => c.IdProducto == cursoId);

            if (curso == null)
                throw new Exception($"Curso {cursoId} no encontrado.");

            // Producto
            curso.Producto.Nombre = dto.Titulo ?? curso.Producto.Nombre;
            curso.Producto.Descripcion = dto.Descripcion ?? curso.Producto.Descripcion;
            curso.Producto.Precio = dto.Precio != 0 ? dto.Precio : curso.Producto.Precio;

            // CursoPregrabado
            curso.CursoPregrabado.PrecioPuntos =
                dto.PrecioPuntos != 0 ? dto.PrecioPuntos : curso.CursoPregrabado.PrecioPuntos;

            // Portada nueva?
            if (dto.Portada != null)
            {
                var nuevaKey = await _storage.UploadImageAsync(dto.Portada);
                curso.CursoPregrabado.UrlPortada = nuevaKey;
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            if (transaction.GetDbTransaction().Connection != null)
            {
                await transaction.RollbackAsync();
            }
            throw;
        }
    }

    public async Task ModificarCapituloAsync(int capituloId, string nuevoNombre)
    {
        var capitulo = await _context.Capitulos.FindAsync(capituloId);

        if (capitulo == null)
            throw new Exception($"Capítulo {capituloId} no encontrado.");

        capitulo.Nombre = nuevoNombre;

        await _context.SaveChangesAsync();
    }

    public async Task ModificarVideoAsync(int videoId, VideoDto dto)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var video = await _context.Videos
                .Include(v => v.Recurso)
                .FirstOrDefaultAsync(v => v.IdRecurso == videoId);

            if (video == null)
                throw new Exception($"Video {videoId} no encontrado.");

            // 1. Cambiar nombre
            if (!string.IsNullOrWhiteSpace(dto.Nombre))
                video.Recurso.Nombre = dto.Nombre;

            // 2. ¿Reemplazar archivo?
            if (dto.Archivo != null)
            {
                // Subir nuevo video
                var newKey = await _storage.UploadVideoAsync(dto.Archivo);
                video.Recurso.Url = newKey;

                // Guardar temporalmente para procesar con FFmpeg
                var tempVideoPath = Path.Combine(Path.GetTempPath(), dto.Archivo.FileName);
                using (var stream = new FileStream(tempVideoPath, FileMode.Create))
                {
                    await dto.Archivo.CopyToAsync(stream);
                }

                // Obtener duración y peso
                var mediaInfo = await FFmpeg.GetMediaInfo(tempVideoPath);
                video.DuracionSegundos = (decimal)mediaInfo.Duration.TotalSeconds;
                video.PesoBytes = new FileInfo(tempVideoPath).Length;

                // Miniatura
                if (dto.Miniatura != null)
                {
                    video.MiniaturaUrl = await _storage.UploadImageAsync(dto.Miniatura);
                }
                else
                {
                    var tempThumbPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.jpg");
                    var conversion = FFmpeg.Conversions.New()
                        .AddParameter($"-ss {video.DuracionSegundos / 2} -i {tempVideoPath} -frames:v 1 {tempThumbPath}");
                    await conversion.Start();

                    using var thumbStream = new FileStream(tempThumbPath, FileMode.Open);
                    var formFile = new FormFile(thumbStream, 0, thumbStream.Length, null, Path.GetFileName(tempThumbPath));
                    video.MiniaturaUrl = await _storage.UploadImageAsync(formFile);
                }
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            if (transaction.GetDbTransaction().Connection != null)
            {
                await transaction.RollbackAsync();
            }
            throw;
        }
    }


    public async Task EliminarVideoAsync(int videoId)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var video = await _context.Videos
                .Include(v => v.Recurso)
                .FirstOrDefaultAsync(v => v.IdRecurso == videoId);

            if (video == null)
                throw new Exception($"Video {videoId} no encontrado.");

            var keyVideo = video.Recurso.Url;
            var keyMiniatura = video.MiniaturaUrl;

            _context.Recursos.Remove(video.Recurso);
            _context.Videos.Remove(video);

            await _context.SaveChangesAsync();

            // Eliminar físicamente del bucket
            await _storage.DeleteVideoAsync(keyVideo);
            if (!string.IsNullOrEmpty(keyMiniatura))
                 await _storage.DeleteMiniaturaAsync(keyMiniatura);

            await transaction.CommitAsync();
        }
        catch
        {
            if (transaction.GetDbTransaction().Connection != null)
            {
                await transaction.RollbackAsync();
            }
            throw;
        }
    }


    public async Task EliminarCapituloAsync(int capituloId)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var capitulo = await _context.Capitulos
                .Include(c => c.Videos)
                    .ThenInclude(v => v.Recurso)
                .FirstOrDefaultAsync(c => c.IdCapitulo == capituloId);

            if (capitulo == null)
                throw new Exception($"Capítulo {capituloId} no encontrado.");

            var cursoId = capitulo.IdCursoPregrabado;

            // =====================================
            // 1. Eliminar videos y recursos
            // =====================================
            foreach (var video in capitulo.Videos)
            {
                var key = video.Recurso.Url;
                var miniaturaKey = video.MiniaturaUrl;

                _context.Recursos.Remove(video.Recurso);
                _context.Videos.Remove(video);

                await _storage.DeleteVideoAsync(key);
                if (!string.IsNullOrEmpty(miniaturaKey))
                    await _storage.DeleteMiniaturaAsync(miniaturaKey);
            }

            // =====================================
            // 2. Eliminar capítulo
            // =====================================
            _context.Capitulos.Remove(capitulo);
            await _context.SaveChangesAsync();

            // =====================================
            // 3. Reordenar capítulos restantes
            // =====================================
            var capitulosRestantes = await _context.Capitulos
                .Where(c => c.IdCursoPregrabado == cursoId)
                .OrderBy(c => c.NumeroOrden)
                .ToListAsync();

            int numero = 1;
            foreach (var c in capitulosRestantes)
            {
                c.NumeroOrden = numero++;
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            if (transaction.GetDbTransaction().Connection != null)
            {
                await transaction.RollbackAsync();
            }
            throw;
        }
    }

    public async Task EliminarCursoAsync(int cursoId)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            // =========================================
            // 1. Cargar curso + producto + pregrabado
            // =========================================
            var curso = await _context.Cursos
                .Include(c => c.Producto)
                .Include(c => c.CursoPregrabado)
                    .ThenInclude(cp => cp.Capitulos)
                        .ThenInclude(cap => cap.Videos)
                            .ThenInclude(v => v.Recurso)
                .FirstOrDefaultAsync(c => c.IdProducto == cursoId);

            if (curso == null)
                throw new Exception($"El curso {cursoId} no existe.");

            // =========================================
            // 2. Si tiene portada → eliminar archivo MinIO
            // =========================================
            if (curso.CursoPregrabado?.UrlPortada != null)
            {
                try
                {
                    // borrar físicamente del bucket
                     await _storage.DeleteImageAsync(curso.CursoPregrabado.UrlPortada);
                }
                catch
                {
                    // opcional: log
                }
            }

            // =========================================
            // 3. Eliminar capítulos → videos → recursos
            // =========================================
            var capitulos = curso.CursoPregrabado?.Capitulos?.ToList() ?? new List<Capitulo>();

            foreach (var capitulo in capitulos)
            {
                foreach (var video in capitulo.Videos)
                {
                    var keyVideo = video.Recurso.Url;
                    var keyMiniatura = video.MiniaturaUrl;

                    _context.Recursos.Remove(video.Recurso);
                    _context.Videos.Remove(video);

                    await _storage.DeleteVideoAsync(keyVideo);
                    if (!string.IsNullOrEmpty(keyMiniatura))
                         await _storage.DeleteMiniaturaAsync(keyMiniatura);
                }


                _context.Capitulos.Remove(capitulo);
            }

            // =========================================
            // 4. Eliminar el curso pregrabado
            // =========================================
            if (curso.CursoPregrabado != null)
                _context.CursoPregrabados.Remove(curso.CursoPregrabado);

            // =========================================
            // 5. Eliminar el curso
            // =========================================
            _context.Cursos.Remove(curso);

            // =========================================
            // 6. Eliminar producto asociado
            // =========================================
            if (curso.Producto != null)
                _context.Productos.Remove(curso.Producto);

            // =========================================
            // 7. Guardar y confirmar transacción
            // =========================================
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            if (transaction.GetDbTransaction().Connection != null)
            {
                await transaction.RollbackAsync();
            }
            throw;
        }
    }

}
