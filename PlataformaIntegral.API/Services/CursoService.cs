using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client.Extensions.Msal;
using PlataformaIntegral.API.DTOs;
using PlataformaIntegral.API.Models;
using System;

namespace PlataformaIntegral.API.Services
{
    public class CursoService : ICursoService
    {
        private readonly PlataformaIntegralContext _context;
        private readonly IMapper _mapper;
        private readonly MinioService _storage; // servicio de almacenamiento para URLs presignadas

        public CursoService(PlataformaIntegralContext context, IMapper mapper, MinioService storage)
        {
            _context = context;
            _mapper = mapper;
            _storage = storage;
        }

        // ---------------------------
        // 0. HELPERS
        // ---------------------------
        private async Task AsignarPortadasPresignadasAsync(IEnumerable<CursoCardDto> cursos)
        {
            TimeSpan expiry = TimeSpan.FromMinutes(60);

            var tareas = cursos
                .Where(c => !string.IsNullOrEmpty(c.PortadaUrl) &&
                            c.PortadaUrl != "ninguna" &&
                            c.PortadaUrl != "null")
                .Select(async curso =>
                {
                    try
                    {
                        curso.PortadaUrl =
                            await _storage.GetImageUrlAsync(curso.PortadaUrl!, expiry);
                    }
                    catch (Exception ex)
                    {
                        // Opcional: log
                        //_logger.LogWarning(ex, $"Portada no encontrada: {curso.PortadaUrl}");

                        curso.PortadaUrl = null;  // o mantener la original
                    }
                });

            await Task.WhenAll(tareas);
        }

        private async Task<bool> ValidarAccesoCursoAsync(int cursoId, int usuarioId, string? rol)
        {
            if (string.Equals(rol, "Administrador", StringComparison.OrdinalIgnoreCase))
                return true;

            if (string.Equals(rol, "Profesor", StringComparison.OrdinalIgnoreCase))
            {
                return await _context.Cursos
                    .AnyAsync(c =>
                        c.IdProducto == cursoId &&
                        c.ProfesorCursos.Any(pc => pc.IdProfesor == usuarioId)
                    );
            }

            return await _context.Pagos.AnyAsync(p =>
                p.IdProducto == cursoId &&
                p.EstadoPago.Nombre == "Aprobado" &&
                (
                    (p.Recibo != null && p.Recibo.IdUsuario == usuarioId) ||
                    (p.IdUsuario == usuarioId)
                )
            );
        }


        // ---------------------------
        // 1. Cursos Populares
        // ---------------------------
        public async Task<List<CursoCardDto>> ObtenerCursosPopularesAsync(int limite)
        {
            var cursos = await _context.Cursos
                .OrderByDescending(c => c.Producto.Pagos.Count(p => p.EstadoPago.Nombre == "Aprobado"))
                .Take(limite)
                .ToListAsync();

            cursos = await _context.Cursos
                .Include(c => c.Producto)              // necesario para Precio y Titulo
                .Include(c => c.CursoPregrabado)       // necesario para PortadaUrl
                .Include(c => c.Categorias)            // necesario para Categorias
                .Include(c => c.ProfesorCursos)
                    .ThenInclude(pc => pc.Profesor)   // necesario para Profesores
                .ThenInclude(p => p.Usuario)          // si quieres Nombre y Apellido
                .ToListAsync();

            var dtos = _mapper.Map<List<CursoCardDto>>(cursos);

            await AsignarPortadasPresignadasAsync(dtos);

            return dtos;
        }


        // ---------------------------
        // 2. Cursos Nuevos
        // ---------------------------
        public async Task<List<CursoCardDto>> ObtenerCursosNuevosAsync(int limite)
        {
            var cursos = await _context.Cursos
                .OrderByDescending(c => c.Producto.FechaCreacion)
                .Take(limite)
                .ToListAsync();

            cursos = await _context.Cursos
                .Include(c => c.Producto)              // necesario para Precio y Titulo
                .Include(c => c.CursoPregrabado)       // necesario para PortadaUrl
                .Include(c => c.Categorias)            // necesario para Categorias
                .Include(c => c.ProfesorCursos)
                    .ThenInclude(pc => pc.Profesor)   // necesario para Profesores
                .ThenInclude(p => p.Usuario)          // si quieres Nombre y Apellido
                .ToListAsync();

            var dtos = _mapper.Map<List<CursoCardDto>>(cursos);

            await AsignarPortadasPresignadasAsync(dtos);

            return dtos;
        }


        // ---------------------------
        // 3. Búsqueda general
        // ---------------------------
        public async Task<List<CursoCardDto>> BuscarCursosAsync(string? texto, int pagina, int tamañoPagina)
        {
            var query = _context.Cursos.AsQueryable();

            if (!string.IsNullOrWhiteSpace(texto))
            {
                string filtro = texto.Trim().ToLower();

                query = query.Where(c =>
                    c.Producto.Nombre.ToLower().Contains(filtro) ||
                    (c.Producto.Descripcion != null && c.Producto.Descripcion.ToLower().Contains(filtro))
                );
            }

            var cursos = await query
                .OrderBy(c => c.Producto.Nombre)
                .Skip((pagina - 1) * tamañoPagina)
                .Take(tamañoPagina)
                .ToListAsync();

            cursos = await _context.Cursos
                .Include(c => c.Producto)              // necesario para Precio y Titulo
                .Include(c => c.CursoPregrabado)       // necesario para PortadaUrl
                .Include(c => c.Categorias)            // necesario para Categorias
                .Include(c => c.ProfesorCursos)
                    .ThenInclude(pc => pc.Profesor)   // necesario para Profesores
                .ThenInclude(p => p.Usuario)          // si quieres Nombre y Apellido
                .ToListAsync();

            cursos = await _context.Cursos
                .Include(c => c.Producto)              // necesario para Precio y Titulo
                .Include(c => c.CursoPregrabado)       // necesario para PortadaUrl
                .Include(c => c.Categorias)            // necesario para Categorias
                .Include(c => c.ProfesorCursos)
                    .ThenInclude(pc => pc.Profesor)   // necesario para Profesores
                .ThenInclude(p => p.Usuario)          // si quieres Nombre y Apellido
                .ToListAsync();

            var dtos = _mapper.Map<List<CursoCardDto>>(cursos);

            await AsignarPortadasPresignadasAsync(dtos);

            return dtos;
        }


        // ---------------------------
        // 4. Cursos por categoría
        // ---------------------------
        public async Task<List<CursoCardDto>> ObtenerCursosPorCategoriaAsync(int categoriaId, int pagina, int tamañoPagina)
        {
            var cursos = await _context.Cursos
                .Where(c => c.Categorias.Any(cc => cc.IdCategoria == categoriaId))
                .Skip((pagina - 1) * tamañoPagina)
                .Take(tamañoPagina)
                .ToListAsync();

            cursos = await _context.Cursos
                .Include(c => c.Producto)              // necesario para Precio y Titulo
                .Include(c => c.CursoPregrabado)       // necesario para PortadaUrl
                .Include(c => c.Categorias)            // necesario para Categorias
                .Include(c => c.ProfesorCursos)
                    .ThenInclude(pc => pc.Profesor)   // necesario para Profesores
                .ThenInclude(p => p.Usuario)          // si quieres Nombre y Apellido
                .ToListAsync();

            var dtos = _mapper.Map<List<CursoCardDto>>(cursos);

            await AsignarPortadasPresignadasAsync(dtos);

            return dtos;
        }


        // ---------------------------
        // 5. Obtener Card por ID
        // ---------------------------
        public async Task<CursoCardDto?> ObtenerCursoCardPorIdAsync(int cursoId)
        {
            var curso = await _context.Cursos
                .FirstOrDefaultAsync(c => c.IdProducto == cursoId);

            if (curso == null) return null;

            var dto = _mapper.Map<CursoCardDto>(curso);

            if (!string.IsNullOrEmpty(dto.PortadaUrl))
                dto.PortadaUrl = await _storage.GetImageUrlAsync(dto.PortadaUrl, TimeSpan.FromMinutes(60));

            return dto;
        }


        public async Task<List<CursoCardDto>> ObtenerCursosPorUsuarioAsync(int usuarioId)
        {
            var cursos = await _context.Cursos
                .Where(c =>
                    c.Producto.Pagos.Any(p =>
                        p.EstadoPago.Nombre == "Aprobado" &&
                        (
                            (p.Recibo != null && p.Recibo.IdUsuario == usuarioId) // caso recibo
                            || (p.IdUsuario == usuarioId)  // caso FK directa en pago
                        )
                    )
                )
                .ToListAsync();

            cursos = await _context.Cursos
                .Include(c => c.Producto)              // necesario para Precio y Titulo
                .Include(c => c.CursoPregrabado)       // necesario para PortadaUrl
                .Include(c => c.Categorias)            // necesario para Categorias
                .Include(c => c.ProfesorCursos)
                    .ThenInclude(pc => pc.Profesor)   // necesario para Profesores
                .ThenInclude(p => p.Usuario)          // si quieres Nombre y Apellido
                .ToListAsync();

            return _mapper.Map<List<CursoCardDto>>(cursos);
        }


        public async Task<CursoPaginaDto?> ObtenerPaginaCursoAsync(int cursoId, int? usuarioId = null, string? rol = null)
        {
            var cursoQuery = await _context.Cursos
                .Where(c => c.IdProducto == cursoId)
                .Select(c => new
                {
                    ProductoId = c.IdProducto,
                    Titulo = c.Producto.Nombre,
                    Descripcion = c.Producto.Descripcion,
                    PortadaUrl = c.CursoPregrabado != null ? c.CursoPregrabado.UrlPortada : "ninguna",
                    Precio = c.CursoPregrabado != null ? c.Producto.Precio : 0,
                    PrecioPuntos = c.CursoPregrabado != null ? c.CursoPregrabado.PrecioPuntos : 0,
                    Categorias = c.Categorias.Select(cc => cc.Nombre).ToList(),
                    FechaCreacion = c.Producto.FechaCreacion,
                    Profesores = c.ProfesorCursos.Select(pc => new {
                        IdProfesor = pc.Profesor.IdUsuario,
                        Nombre = pc.Profesor.Usuario.Nombre,
                        Apellido = pc.Profesor.Usuario.Apellido
                    }).ToList(),

                    Capitulos = c.CursoPregrabado.Capitulos.OrderBy(cp => cp.NumeroOrden).Select(cp => new
                    {
                        cp.IdCapitulo,
                        cp.Nombre,
                        cp.NumeroOrden,
                        Videos = cp.Videos.OrderBy(v => v.NumeroOrden)
                            .Select(v => new {
                                v.IdRecurso,
                                v.Recurso.Nombre,
                                v.NumeroOrden,
                                v.MiniaturaUrl,
                                v.DuracionSegundos,
                                v.PesoBytes
                            }).ToList(),
                        Cuestionarios = cp.Cuestionarios.OrderBy(q => q.NumeroOrden)
                            .Select(q => new { q.IdRecurso, q.Recurso.Nombre, q.NumeroOrden }).ToList()
                    }).ToList(),

                    ProfesoresResenas = c.ProfesorCursos
                        .SelectMany(pc => pc.Profesor.ReseñasProfesor)
                        .Select(r => new { r.Opinion })
                        .ToList(),
                    ReseñaUsuario =
                        usuarioId.HasValue
                        ? c.ReseñaCursos
                            .Where(r => r.IdEstudiante == usuarioId.Value)
                            .Select(r => (bool?)r.Opinion)
                            .FirstOrDefault()
                        : null,
                    CalificacionGeneral =
                        c.ReseñaCursos.Any()
                            ? (double?)c.ReseñaCursos.Count(r => r.Opinion) * 100.0 / c.ReseñaCursos.Count()
                            : null,
                })
                .FirstOrDefaultAsync();

            if (cursoQuery == null) return null;

            // Progreso del usuario
            Dictionary<int, string?> progreso = new();
            if (usuarioId.HasValue)
            {
                var recursoIds = cursoQuery.Capitulos
                    .SelectMany(cp => cp.Videos.Select(v => v.IdRecurso).Concat(cp.Cuestionarios.Select(q => q.IdRecurso)))
                    .Distinct()
                    .ToList();

                if (recursoIds.Any())
                {
                    progreso = await _context.EstudianteProgresos
                        .Where(ep => ep.IdEstudiante == usuarioId.Value && recursoIds.Contains(ep.IdRecurso))
                        .ToDictionaryAsync(ep => ep.IdRecurso, ep => ep.Estado);
                }
            }

            bool tieneAcceso = usuarioId.HasValue
                ? await ValidarAccesoCursoAsync(cursoId, usuarioId ?? 0, rol)
                : rol == "Administrador";

            var cursoDto = new CursoPaginaDto
            {
                Id = cursoQuery.ProductoId,
                Titulo = cursoQuery.Titulo,
                Descripcion = cursoQuery.Descripcion,
                PortadaUrl = cursoQuery.PortadaUrl,
                Precio = cursoQuery.Precio,
                PrecioPuntos = cursoQuery.PrecioPuntos,
                Comprado = tieneAcceso,
                Categorias = cursoQuery.Categorias,
                FechaCreacion = cursoQuery.FechaCreacion,
                Profesores = cursoQuery.Profesores.Select(p => new ProfesorSimpleDto
                {
                    Id = p.IdProfesor,
                    NombreCompleto = p.Nombre
                }).ToList(),
                Calificacion = cursoQuery.CalificacionGeneral,
                Reseña = cursoQuery.ReseñaUsuario,
                Capitulos = cursoQuery.Capitulos.Select(cp => new CapituloDto
                {
                    Id = cp.IdCapitulo,
                    Titulo = cp.Nombre,
                    NumeroOrden = cp.NumeroOrden,
                    Videos = cp.Videos.Select(v => new RecursoVideoDto
                    {
                        Id = v.IdRecurso,
                        Titulo = v.Nombre,
                        NumeroOrden = v.NumeroOrden,
                        Duracion = TimeSpan.FromSeconds((double)v.DuracionSegundos),
                        PesoBytes = v.PesoBytes,
                        MiniaturaUrl = string.IsNullOrEmpty(v.MiniaturaUrl) ? null :
                            _storage.GetMiniaturaUrlAsync(v.MiniaturaUrl, TimeSpan.FromMinutes(60)).Result,
                        Visto = progreso.ContainsKey(v.IdRecurso) && progreso[v.IdRecurso] == "Visto"
                    }).ToList(),
                    Cuestionarios = cp.Cuestionarios.Select(q => new RecursoCuestionarioDto
                    {
                        Id = q.IdRecurso,
                        Titulo = q.Nombre,
                        NumeroOrden = q.NumeroOrden,
                        Resuelto = progreso.ContainsKey(q.IdRecurso) && progreso[q.IdRecurso] == "Completado"
                    }).ToList()
                }).ToList()
            };

            if (!string.IsNullOrEmpty(cursoDto.PortadaUrl))
            {
                cursoDto.PortadaUrl = await _storage.GetImageUrlAsync(cursoDto.PortadaUrl, TimeSpan.FromMinutes(60));
            }

            return cursoDto;
        }

        public async Task<List<Uri>> ObtenerUrlsDescargaCursoAsync(int cursoId, int usuarioId, int minutesExpiry, string? rol = null)
        {
            // Validar que es curso pregrabado
            var cursoPre = await _context.Cursos
                .Include(c => c.CursoPregrabado)
                .FirstOrDefaultAsync(c => c.IdProducto == cursoId);

            if (cursoPre?.CursoPregrabado == null)
                return new List<Uri>();

            // Validar acceso con helper
            bool tieneAcceso = await ValidarAccesoCursoAsync(cursoId, usuarioId, rol);

            if (!tieneAcceso)
                return new List<Uri>();

            // Obtener recursos (videos y documentos)
            var recursos = await _context.Capitulos
                .Where(cp => cp.IdCursoPregrabado == cursoPre.CursoPregrabado.IdCurso)
                .SelectMany(cp => cp.Videos.Select(v => new { v.Recurso.Url, Tipo = "videos" })
                    .Concat(cp.Cuestionarios.Select(q => new { q.Recurso.Url, Tipo = "documentos" })))
                .ToListAsync();

            var urls = new List<Uri>();
            TimeSpan expiry = TimeSpan.FromMinutes(minutesExpiry);

            foreach (var recurso in recursos)
            {
                var presignedUrl = new Uri(await _storage.GetFileUrlAsync(recurso.Tipo, recurso.Url, expiry));
                urls.Add(presignedUrl);
            }

            return urls;
        }

        public async Task<VideoDetalleDto?> ObtenerVideoDetalleAsync(
    int recursoId, // Id del Recurso, que también es Id del Video
    int usuarioId,
    int minutesUrlExpiry,
    string? rol = null)
        {
            // 1) Obtener video + recurso + curso
            var videoData = await _context.Videos
                .AsNoTracking()
                .Where(v => v.IdRecurso == recursoId) // relación 1:1
                .Select(v => new
                {
                    v.IdRecurso,
                    v.Recurso.Nombre,
                    v.Descripcion,
                    v.DuracionSegundos,
                    v.PesoBytes,
                    v.MiniaturaUrl,
                    v.Recurso.Url,
                    CursoId = v.Capitulo.CursoPregrabado.Curso.IdProducto,
                    NumeroOrdenVideo = v.NumeroOrden,
                    NumeroOrdenCapitulo = v.Capitulo.NumeroOrden
                })
                .FirstOrDefaultAsync();

            if (videoData == null)
                return null;

            // 2) Determinar si es el primer video del curso
            var primerVideoId = await _context.Videos
                .Where(v => v.Capitulo.CursoPregrabado.Curso.IdProducto == videoData.CursoId)
                .OrderBy(v => v.Capitulo.NumeroOrden)
                .ThenBy(v => v.NumeroOrden)
                .Select(v => v.IdRecurso)
                .FirstOrDefaultAsync();

            bool esPrimerVideo = videoData.IdRecurso == primerVideoId;

            // 3) Validar acceso del usuario al curso (excepto si es el primer video)
            bool tieneAcceso = esPrimerVideo || await ValidarAccesoCursoAsync(videoData.CursoId, usuarioId, rol);

            if (!tieneAcceso)
                return null;

            // 4) Generar URL presignada
            var urlExpiry = TimeSpan.FromMinutes(minutesUrlExpiry);
            var presignedUrl = await _storage.GetVideoUrlAsync(videoData.Url, urlExpiry);
            var miniaturaUrl = await _storage.GetMiniaturaUrlAsync(videoData.MiniaturaUrl, urlExpiry);

            // 5) Construir DTO
            var videoDetalleDto = new VideoDetalleDto
            {
                Id = videoData.IdRecurso,
                Titulo = videoData.Nombre,
                Descripcion = videoData.Descripcion,
                Duracion = TimeSpan.FromSeconds((double)videoData.DuracionSegundos),
                MiniaturaUrl = miniaturaUrl,
                PesoBytes = videoData.PesoBytes,
                VideoUrl = presignedUrl
            };

            return videoDetalleDto;
        }

    }
}
