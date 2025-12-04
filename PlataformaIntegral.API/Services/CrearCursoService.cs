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

            string? portadaUrl = null;

            if (dto.Portada != null)
            {
                // Validar extensión y MIME type
                var extensionesPermitidas = new[] { ".jpg", ".jpeg", ".png", ".bmp", ".gif", ".tiff", ".ico" };
                var extension = Path.GetExtension(dto.Portada.FileName).ToLowerInvariant();

                if (!extensionesPermitidas.Contains(extension))
                {
                    throw new InvalidOperationException(
                        $"Formato de imagen no permitido: {extension}. Solo se aceptan {string.Join(", ", extensionesPermitidas)}");
                }

                // Opcional: validar MIME type también
                var mimePermitidos = new[] { "image/jpeg", "image/png", "image/bmp", "image/gif", "image/tiff", "image/x-icon" };
                if (!mimePermitidos.Contains(dto.Portada.ContentType.ToLowerInvariant()))
                {
                    throw new InvalidOperationException(
                        $"Tipo MIME no permitido: {dto.Portada.ContentType}. Solo se aceptan {string.Join(", ", mimePermitidos)}");
                }

                portadaUrl = await _storage.UploadImageAsync(dto.Portada);
            }

            var cursoPregrabado = new CursoPregrabado
            {
                IdCurso = curso.IdProducto,
                Curso = curso,
                PrecioPuntos = dto.PrecioPuntos,
                UrlPortada = portadaUrl
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
        if (string.IsNullOrWhiteSpace(nombreCapitulo))
            throw new ArgumentException("El nombre del capítulo no puede estar vacío.", nameof(nombreCapitulo));

        if (nombreCapitulo.Length > 200)
            throw new ArgumentException("El nombre del capítulo excede la longitud máxima permitida (200 caracteres).", nameof(nombreCapitulo));

        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            // Validar que el curso pregrabado exista
            var cursoPregrabado = await _context.CursoPregrabados
                .FirstOrDefaultAsync(cp => cp.IdCurso == cursoPregrabadoId);

            if (cursoPregrabado == null)
                throw new InvalidOperationException($"Curso pregrabado con ID {cursoPregrabadoId} no encontrado.");

            // Calcular número de orden
            var numero = await _context.Capitulos
                .CountAsync(c => c.IdCursoPregrabado == cursoPregrabadoId) + 1;

            var capitulo = new Capitulo
            {
                IdCursoPregrabado = cursoPregrabadoId,
                Nombre = nombreCapitulo.Trim(),
                NumeroOrden = numero
            };

            _context.Capitulos.Add(capitulo);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
            return capitulo.IdCapitulo;
        }
        catch
        {
            if (transaction.GetDbTransaction()?.Connection != null)
            {
                await transaction.RollbackAsync();
            }
            throw;
        }
    }

    // ========================================
    // 3. SUBIR VIDEO
    // ========================================
    public async Task<int> SubirVideoAsync(int capituloId, [FromForm] VideoDto dto)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            // 1) Validar formato de video
            var extensionesVideoPermitidas = new[] { ".mp4", ".mov", ".avi", ".wmv", ".mkv" };
            var extensionVideo = Path.GetExtension(dto.Archivo.FileName).ToLowerInvariant();
            if (!extensionesVideoPermitidas.Contains(extensionVideo))
            {
                throw new InvalidOperationException(
                    $"Formato de video no permitido: {extensionVideo}. Solo se aceptan {string.Join(", ", extensionesVideoPermitidas)}");
            }

            var mimeVideoPermitidos = new[] { "video/mp4", "video/x-msvideo", "video/x-ms-wmv", "video/quicktime", "video/x-matroska" };
            if (!mimeVideoPermitidos.Contains(dto.Archivo.ContentType.ToLowerInvariant()))
            {
                throw new InvalidOperationException(
                    $"Tipo MIME de video no permitido: {dto.Archivo.ContentType}. Solo se aceptan {string.Join(", ", mimeVideoPermitidos)}");
            }

            // 2) Subir video a MinIO
            string videoKey = await _storage.UploadVideoAsync(dto.Archivo);

            // Guardar temporalmente para procesar con FFmpeg
            var tempVideoPath = Path.Combine(Path.GetTempPath(), dto.Archivo.FileName);
            using (var stream = new FileStream(tempVideoPath, FileMode.Create))
            {
                await dto.Archivo.CopyToAsync(stream);
            }

            // 3) Obtener duración y peso
            var mediaInfo = await FFmpeg.GetMediaInfo(tempVideoPath);
            var duracion = (int)mediaInfo.Duration.TotalSeconds;
            var peso = new FileInfo(tempVideoPath).Length;

            // 4) Miniatura
            string miniaturaKey;
            if (dto.Miniatura != null)
            {
                // Validar formato de miniatura
                var extensionesImgPermitidas = new[] { ".jpg", ".jpeg", ".png", ".bmp", ".gif", ".tiff", ".ico" };
                var extensionImg = Path.GetExtension(dto.Miniatura.FileName).ToLowerInvariant();
                if (!extensionesImgPermitidas.Contains(extensionImg))
                {
                    throw new InvalidOperationException(
                        $"Formato de imagen no permitido: {extensionImg}. Solo se aceptan {string.Join(", ", extensionesImgPermitidas)}");
                }

                var mimeImgPermitidos = new[] { "image/jpeg", "image/png", "image/bmp", "image/gif", "image/tiff", "image/x-icon" };
                if (!mimeImgPermitidos.Contains(dto.Miniatura.ContentType.ToLowerInvariant()))
                {
                    throw new InvalidOperationException(
                        $"Tipo MIME de imagen no permitido: {dto.Miniatura.ContentType}. Solo se aceptan {string.Join(", ", mimeImgPermitidos)}");
                }

                miniaturaKey = await _storage.UploadMiniaturaAsync(dto.Miniatura);
            }
            else
            {
                // Generar miniatura automática en JPG
                var tempThumbPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.jpg");
                var conversion = FFmpeg.Conversions.New()
                    .AddParameter($"-ss {duracion / 2} -i {tempVideoPath} -frames:v 1 {tempThumbPath}");
                await conversion.Start();

                using var thumbStream = new FileStream(tempThumbPath, FileMode.Open, FileAccess.Read);
                var formFile = new FormFile(thumbStream, 0, thumbStream.Length, "miniatura", Path.GetFileName(tempThumbPath))
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "image/jpeg"
                };

                miniaturaKey = await _storage.UploadMiniaturaAsync(formFile);
            }

            // 5) Crear entidad Recurso
            var recurso = new Recurso
            {
                Nombre = dto.Nombre,
                Url = videoKey
            };

            _context.Recursos.Add(recurso);
            await _context.SaveChangesAsync();

            // 6) Crear entidad Video
            var numeroOrden = await _context.Videos
                .CountAsync(v => v.IdCapitulo == capituloId) + 1;

            var video = new Video
            {
                Descripcion = dto.Descripcion,
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
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var curso = await _context.Cursos.FindAsync(cursoId);

            if (curso == null)
                throw new InvalidOperationException($"Curso con ID {cursoId} no encontrado.");

            // Validar estado actual
            if (curso.Estado == EstadoCursoEnum.Publicado)
                throw new InvalidOperationException($"El curso {cursoId} ya está publicado.");

            if (curso.Estado == EstadoCursoEnum.Borrador || curso.Estado == EstadoCursoEnum.PendienteRevision)
            {
                curso.Estado = EstadoCursoEnum.Publicado;
            }
            else
            {
                throw new InvalidOperationException(
                    $"El curso {cursoId} no puede publicarse desde el estado {curso.Estado}.");
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            if (transaction.GetDbTransaction()?.Connection != null)
            {
                await transaction.RollbackAsync();
            }
            throw;
        }
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
                throw new InvalidOperationException($"Curso {cursoId} no encontrado.");

            // Validar precios
            if (dto.Precio < 0)
                throw new InvalidOperationException("El precio no puede ser negativo.");
            if (dto.PrecioPuntos < 0)
                throw new InvalidOperationException("El precio en puntos no puede ser negativo.");

            // Producto
            if (!string.IsNullOrWhiteSpace(dto.Titulo))
                curso.Producto.Nombre = dto.Titulo;

            if (!string.IsNullOrWhiteSpace(dto.Descripcion))
                curso.Producto.Descripcion = dto.Descripcion;

            if (dto.Precio > 0)
                curso.Producto.Precio = dto.Precio;

            // CursoPregrabado
            if (dto.PrecioPuntos > 0)
                curso.CursoPregrabado.PrecioPuntos = dto.PrecioPuntos;

            // Portada nueva?
            if (dto.Portada != null)
            {
                var extensionesPermitidas = new[] { ".jpg", ".jpeg", ".png", ".bmp", ".gif", ".tiff", ".ico" };
                var extension = Path.GetExtension(dto.Portada.FileName).ToLowerInvariant();

                if (!extensionesPermitidas.Contains(extension))
                {
                    throw new InvalidOperationException(
                        $"Formato de imagen no permitido: {extension}. Solo se aceptan {string.Join(", ", extensionesPermitidas)}");
                }

                var mimePermitidos = new[] { "image/jpeg", "image/png", "image/bmp", "image/gif", "image/tiff", "image/x-icon" };
                if (!mimePermitidos.Contains(dto.Portada.ContentType.ToLowerInvariant()))
                {
                    throw new InvalidOperationException(
                        $"Tipo MIME no permitido: {dto.Portada.ContentType}. Solo se aceptan {string.Join(", ", mimePermitidos)}");
                }

                var nuevaKey = await _storage.UploadImageAsync(dto.Portada);
                curso.CursoPregrabado.UrlPortada = nuevaKey;
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            if (transaction.GetDbTransaction()?.Connection != null)
            {
                await transaction.RollbackAsync();
            }
            throw;
        }
    }

    public async Task ModificarCapituloAsync(int capituloId, string nuevoNombre)
    {
        if (string.IsNullOrWhiteSpace(nuevoNombre))
            throw new ArgumentException("El nuevo nombre del capítulo no puede estar vacío.", nameof(nuevoNombre));

        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var capitulo = await _context.Capitulos.FindAsync(capituloId);

            if (capitulo == null)
                throw new InvalidOperationException($"Capítulo con ID {capituloId} no encontrado.");

            // Validar longitud máxima (ejemplo: 200 caracteres)
            if (nuevoNombre.Length > 200)
                throw new ArgumentException("El nombre del capítulo excede la longitud máxima permitida (200 caracteres).", nameof(nuevoNombre));

            capitulo.Nombre = nuevoNombre.Trim();

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            if (transaction.GetDbTransaction()?.Connection != null)
            {
                await transaction.RollbackAsync();
            }
            throw;
        }
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
                throw new InvalidOperationException($"Video con ID {videoId} no encontrado.");

            // 1. Cambiar nombre
            if (!string.IsNullOrWhiteSpace(dto.Nombre))
                video.Recurso.Nombre = dto.Nombre.Trim();

            // 2. ¿Reemplazar archivo?
            if (dto.Archivo != null)
            {
                // Validar formato de video
                var extensionesVideoPermitidas = new[] { ".mp4", ".mov", ".avi", ".wmv", ".mkv" };
                var extensionVideo = Path.GetExtension(dto.Archivo.FileName).ToLowerInvariant();
                if (!extensionesVideoPermitidas.Contains(extensionVideo))
                    throw new InvalidOperationException($"Formato de video no permitido: {extensionVideo}");

                var mimeVideoPermitidos = new[] { "video/mp4", "video/x-msvideo", "video/x-ms-wmv", "video/quicktime", "video/x-matroska" };
                if (!mimeVideoPermitidos.Contains(dto.Archivo.ContentType.ToLowerInvariant()))
                    throw new InvalidOperationException($"Tipo MIME de video no permitido: {dto.Archivo.ContentType}");

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
                    // Validar formato de miniatura
                    var extensionesImgPermitidas = new[] { ".jpg", ".jpeg", ".png", ".bmp", ".gif", ".tiff", ".ico" };
                    var extensionImg = Path.GetExtension(dto.Miniatura.FileName).ToLowerInvariant();
                    if (!extensionesImgPermitidas.Contains(extensionImg))
                        throw new InvalidOperationException($"Formato de imagen no permitido: {extensionImg}");

                    var mimeImgPermitidos = new[] { "image/jpeg", "image/png", "image/bmp", "image/gif", "image/tiff", "image/x-icon" };
                    if (!mimeImgPermitidos.Contains(dto.Miniatura.ContentType.ToLowerInvariant()))
                        throw new InvalidOperationException($"Tipo MIME de imagen no permitido: {dto.Miniatura.ContentType}");

                    video.MiniaturaUrl = await _storage.UploadImageAsync(dto.Miniatura);
                }
                else
                {
                    var tempThumbPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.jpg");
                    var conversion = FFmpeg.Conversions.New()
                        .AddParameter($"-ss {video.DuracionSegundos / 2} -i {tempVideoPath} -frames:v 1 {tempThumbPath}");
                    await conversion.Start();

                    using var thumbStream = new FileStream(tempThumbPath, FileMode.Open, FileAccess.Read);
                    var formFile = new FormFile(thumbStream, 0, thumbStream.Length, "miniatura", Path.GetFileName(tempThumbPath))
                    {
                        Headers = new HeaderDictionary(),
                        ContentType = "image/jpeg"
                    };

                    video.MiniaturaUrl = await _storage.UploadImageAsync(formFile);
                }

                // Limpieza opcional de archivos temporales
                try
                {
                    if (File.Exists(tempVideoPath)) File.Delete(tempVideoPath);
                }
                catch { /* Ignorar errores de limpieza */ }
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            if (transaction.GetDbTransaction()?.Connection != null)
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
                .FirstOrDefaultAsync(v => v.IdRecurso == videoId); // usar IdVideo, no IdRecurso

            if (video == null)
                throw new InvalidOperationException($"Video con ID {videoId} no encontrado.");

            var keyVideo = video.Recurso.Url;
            var keyMiniatura = video.MiniaturaUrl;

            // Primero eliminar en BD
            _context.Recursos.Remove(video.Recurso);
            _context.Videos.Remove(video);

            await _context.SaveChangesAsync();

            // Luego intentar eliminar físicamente del bucket
            try
            {
                if (!string.IsNullOrEmpty(keyVideo))
                    await _storage.DeleteVideoAsync(keyVideo);

                if (!string.IsNullOrEmpty(keyMiniatura))
                    await _storage.DeleteMiniaturaAsync(keyMiniatura);
            }
            catch (Exception ex)
            {
                // Aquí puedes loguear el error sin romper la transacción
                // Ejemplo: _logger.LogError(ex, "Error eliminando archivos del bucket");
            }

            await transaction.CommitAsync();
        }
        catch
        {
            if (transaction.GetDbTransaction()?.Connection != null)
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
                throw new InvalidOperationException($"Capítulo con ID {capituloId} no encontrado.");

            var cursoId = capitulo.IdCursoPregrabado;

            // =====================================
            // 1. Eliminar videos y recursos (BD primero)
            // =====================================
            foreach (var video in capitulo.Videos)
            {
                _context.Recursos.Remove(video.Recurso);
                _context.Videos.Remove(video);
            }

            _context.Capitulos.Remove(capitulo);
            await _context.SaveChangesAsync();

            // =====================================
            // 2. Reordenar capítulos restantes
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

            // =====================================
            // 3. Eliminar físicamente del bucket (fuera de la transacción)
            // =====================================
            foreach (var video in capitulo.Videos)
            {
                try
                {
                    if (!string.IsNullOrEmpty(video.Recurso.Url))
                        await _storage.DeleteVideoAsync(video.Recurso.Url);

                    if (!string.IsNullOrEmpty(video.MiniaturaUrl))
                        await _storage.DeleteMiniaturaAsync(video.MiniaturaUrl);
                }
                catch (Exception ex)
                {
                    // Loguear el error sin romper la transacción
                    // Ejemplo: _logger.LogError(ex, $"Error eliminando archivos del bucket para video {video.IdVideo}");
                }
            }

            await transaction.CommitAsync();
        }
        catch
        {
            if (transaction.GetDbTransaction()?.Connection != null)
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
            // 1. Cargar curso completo
            var curso = await _context.Cursos
                .Include(c => c.Producto)
                .Include(c => c.CursoPregrabado)
                    .ThenInclude(cp => cp.Capitulos)
                        .ThenInclude(cap => cap.Videos)
                            .ThenInclude(v => v.Recurso)
                .FirstOrDefaultAsync(c => c.IdProducto == cursoId);

            if (curso == null)
                throw new InvalidOperationException($"El curso con ID {cursoId} no existe.");

            // 2. Eliminar portada (solo BD, storage después)
            var portadaKey = curso.CursoPregrabado?.UrlPortada;

            // 3. Eliminar capítulos, videos y recursos (solo BD)
            var capitulos = curso.CursoPregrabado?.Capitulos?.ToList() ?? new List<Capitulo>();
            var videosKeys = new List<(string? videoKey, string? miniaturaKey)>();

            foreach (var capitulo in capitulos)
            {
                foreach (var video in capitulo.Videos)
                {
                    videosKeys.Add((video.Recurso.Url, video.MiniaturaUrl));

                    _context.Recursos.Remove(video.Recurso);
                    _context.Videos.Remove(video);
                }

                _context.Capitulos.Remove(capitulo);
            }

            // 4. Eliminar curso pregrabado
            if (curso.CursoPregrabado != null)
                _context.CursoPregrabados.Remove(curso.CursoPregrabado);

            // 5. Eliminar curso
            _context.Cursos.Remove(curso);

            // 6. Eliminar producto asociado
            if (curso.Producto != null)
                _context.Productos.Remove(curso.Producto);

            // 7. Guardar cambios en BD
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            // 8. Eliminar físicamente del bucket (fuera de la transacción)
            try
            {
                if (!string.IsNullOrEmpty(portadaKey))
                    await _storage.DeleteImageAsync(portadaKey);

                foreach (var (videoKey, miniaturaKey) in videosKeys)
                {
                    if (!string.IsNullOrEmpty(videoKey))
                        await _storage.DeleteVideoAsync(videoKey);

                    if (!string.IsNullOrEmpty(miniaturaKey))
                        await _storage.DeleteMiniaturaAsync(miniaturaKey);
                }
            }
            catch (Exception ex)
            {
                // Aquí puedes loguear el error sin romper la eliminación en BD
                // Ejemplo: _logger.LogError(ex, $"Error eliminando archivos del bucket para curso {cursoId}");
            }
        }
        catch
        {
            if (transaction.GetDbTransaction()?.Connection != null)
            {
                await transaction.RollbackAsync();
            }
            throw;
        }
    }

}
