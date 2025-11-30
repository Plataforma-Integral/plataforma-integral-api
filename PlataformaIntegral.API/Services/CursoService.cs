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
            // 1) Traer curso con capitulos + recursos (proyección parcial)
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
                    // Profesores desde la relación muchos-a-muchos profesor_curso
                    Profesores = c.ProfesorCursos.Select(pc => new {
                        IdProfesor = pc.Profesor.IdUsuario,
                        Nombre = pc.Profesor.Usuario.Nombre,
                        Apellido = pc.Profesor.Usuario.Apellido,
                        //FotoPerfil = pc.Profesor.Usuario.FotoPerfilUrl
                    }).ToList(),

                    Capitulos = c.CursoPregrabado.Capitulos.OrderBy(cp => cp.NumeroOrden).Select(cp => new
                    {
                        cp.IdCapitulo,
                        cp.Nombre,
                        cp.NumeroOrden,
                        Videos = cp.Videos.OrderBy(r => r.NumeroOrden)
                            .Select(r => new { r.IdRecurso, r.Recurso.Nombre, r.NumeroOrden, Key = r.Recurso.Url }).ToList(),
                        Cuestionarios = cp.Cuestionarios.OrderBy(r => r.NumeroOrden)
                            .Select(r => new { r.IdRecurso, r.Recurso.Nombre, r.NumeroOrden }).ToList()
                    }).ToList(),

                    // Reseñas del profesor(es) - calculamos promedio sobre todas las reseñas de los profesores relacionados
                    ProfesoresResenas = c.ProfesorCursos
                        .SelectMany(pc => pc.Profesor.ReseñasProfesor)
                        .Select(r => new { r.Opinion })
                        .ToList(),
                    // Obtener reseña personal del usuario (si existe)
                    ReseñaUsuario =
                        usuarioId.HasValue
                        ? c.ReseñaCursos
                            .Where(r => r.IdEstudiante == usuarioId.Value)
                            .Select(r => (bool?)r.Opinion)
                            .FirstOrDefault()
                        : null,
                    // Obtener calificación general del curso (0..100)
                    CalificacionGeneral =
                        c.ReseñaCursos.Any()
                            ? (double?)c.ReseñaCursos.Count(r => r.Opinion) * 100.0 / c.ReseñaCursos.Count()
                            : null,

                })
                .FirstOrDefaultAsync();

            if (cursoQuery == null) return null;

            // 2) Obtener progreso del usuario (si aplica)
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

            bool comprado = false;
            if (usuarioId.HasValue)
            {
                comprado = await _context.Pagos
                    .Where(c => c.EstadoPago.Nombre == "Aprobado")
                    .AnyAsync(c => c.IdUsuario == usuarioId.Value && c.IdProducto == cursoId);
            }
            else if(rol == "Administrador")
            {
                comprado = true;
            }

            // 3) Construir DTO final
            var cursoDto = new CursoPaginaDto
            {
                Id = cursoQuery.ProductoId,
                Titulo = cursoQuery.Titulo,
                Descripcion = cursoQuery.Descripcion,
                PortadaUrl = cursoQuery.PortadaUrl,
                Precio = cursoQuery.Precio,
                PrecioPuntos = cursoQuery.PrecioPuntos,
                Comprado = comprado,
                Categorias = cursoQuery.Categorias,
                FechaCreacion = cursoQuery.FechaCreacion,
                Profesores = cursoQuery.Profesores.Select(p => new ProfesorSimpleDto
                {
                    Id = p.IdProfesor,
                    NombreCompleto = p.Nombre,
                    //FotoPerfilUrl = p.FotoPerfil
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
                        //Duracion = v.Duracion,
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
                cursoDto.PortadaUrl = await _storage.GetImageUrlAsync(
                    cursoDto.PortadaUrl,
                    TimeSpan.FromMinutes(60)
                );
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

            // Validar acceso: pago aprobado
            bool comprado = await _context.Pagos.AnyAsync(p =>
                p.IdProducto == cursoId &&
                p.EstadoPago.Nombre == "Aprobado" &&
                (
                    (p.Recibo != null && p.Recibo.IdUsuario == usuarioId) ||
                    (p.IdUsuario == usuarioId)
                )
            );

            if(rol != null && rol == "Administrador")
            {
                comprado = true;
            }

            if (!comprado)
                return new List<Uri>();

            // Obtener recursos (solo videos y documentos)
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

        public async Task<VideoDetalleDto?> ObtenerVideoDetalleAsync(int videoId, int usuarioId, int minutesUrlExpiry, string? rol = null)
        {
            // 1) Obtener video + curso + recurso
            var videoData = await _context.Videos
                .Where(v => v.IdRecurso == videoId)
                .Select(v => new
                {
                    v.IdRecurso,
                    v.Recurso.Nombre,
                    v.Descripcion,
                    v.Recurso.Url,
                    CursoId = v.Capitulo.CursoPregrabado.Curso.IdProducto
                })
                .FirstOrDefaultAsync();
            if (videoData == null)
                return null;
            // 2) Validar acceso del usuario al curso
            bool tieneAcceso = await _context.Pagos.AnyAsync(p =>
                p.IdProducto == videoData.CursoId &&
                p.EstadoPago.Nombre == "Aprobado" &&
                (
                    (p.Recibo != null && p.Recibo.IdUsuario == usuarioId) ||
                    (p.IdUsuario == usuarioId)
                )
            );

            if (!tieneAcceso)
                return null;
            // 3) Generar URL presignada
            TimeSpan urlExpiry = TimeSpan.FromMinutes(minutesUrlExpiry);
            var presignedUrl = await _storage.GetVideoUrlAsync(videoData.Url, urlExpiry);
            // 4) Construir DTO
            var videoDetalleDto = new VideoDetalleDto
            {
                Id = videoData.IdRecurso,
                Titulo = videoData.Nombre,
                Descripcion = videoData.Descripcion,
                PresignedUrl = presignedUrl
            };
            return videoDetalleDto;
        }

    }
}
