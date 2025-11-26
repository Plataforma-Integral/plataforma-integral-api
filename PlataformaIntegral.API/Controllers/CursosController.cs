using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlataformaIntegral.API.DTOs;
using PlataformaIntegral.API.Services;
using System.Security.Claims;

namespace PlataformaIntegral.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize] // 🔒 JWT Authentication required
    public class CursosController : ControllerBase
    {
        private readonly ICursoService _cursoService;

        public CursosController(ICursoService cursoService)
        {
            _cursoService = cursoService;
        }

        // -------------------------------------
        // 1. CURSOS POPULARES
        // -------------------------------------
        [HttpGet("populares")]
        public async Task<IActionResult> ObtenerPopulares([FromQuery] int limite = 10)
        {
            var cursos = await _cursoService.ObtenerCursosPopularesAsync(limite);
            return Ok(cursos);
        }

        // -------------------------------------
        // 2. CURSOS NUEVOS
        // -------------------------------------
        [HttpGet("nuevos")]
        public async Task<IActionResult> ObtenerNuevos([FromQuery] int limite = 10)
        {
            var cursos = await _cursoService.ObtenerCursosNuevosAsync(limite);
            return Ok(cursos);
        }

        // -------------------------------------
        // 3. BÚSQUEDA GENERAL
        // -------------------------------------
        [HttpGet("buscar")]
        public async Task<IActionResult> Buscar(
            [FromQuery] string? texto = null,
            [FromQuery] int pagina = 1,
            [FromQuery(Name = "tamañoPagina")] int tamañoPagina = 12)
        {
            if (pagina < 1 || tamañoPagina < 1)
                return BadRequest("Los parámetros de paginación deben ser mayores a 0.");

            var cursos = await _cursoService.BuscarCursosAsync(texto, pagina, tamañoPagina);
            return Ok(cursos);
        }

        // -------------------------------------
        // 4. CURSOS POR CATEGORÍA
        // -------------------------------------
        [HttpGet("categoria/{categoriaId}")]
        public async Task<IActionResult> ObtenerPorCategoria(
            [FromRoute] int categoriaId,
            [FromQuery] int pagina = 1,
            [FromQuery(Name = "tamañoPagina")] int tamañoPagina = 12)
        {
            if (pagina < 1 || tamañoPagina < 1)
                return BadRequest("Los parámetros de paginación deben ser mayores a 0.");

            var cursos = await _cursoService.ObtenerCursosPorCategoriaAsync(categoriaId, pagina, tamañoPagina);
            return Ok(cursos);
        }

        // -------------------------------------
        // 5. OBTENER UNA CARD DE CURSO POR ID
        // -------------------------------------
        [HttpGet("{cursoId}/card")]
        public async Task<IActionResult> ObtenerCardPorId([FromRoute] int cursoId)
        {
            var curso = await _cursoService.ObtenerCursoCardPorIdAsync(cursoId);

            if (curso == null)
                return NotFound("El curso no existe.");

            return Ok(curso);
        }

        // -------------------------------------
        // 6. OBTENER PÁGINA COMPLETA DE CURSO
        // -------------------------------------
        [HttpGet("{cursoId}/pagina")]
        public async Task<IActionResult> ObtenerPaginaCurso([FromRoute] int cursoId, [FromQuery] int? usuarioId = null)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (usuarioId != userId & userRole != "Administrador")
                return Forbid(); // No puedes modificar otro usuario

            var curso = await _cursoService.ObtenerPaginaCursoAsync(cursoId, usuarioId, userRole);

            if (curso == null)
                return NotFound("El curso no existe.");

            return Ok(curso);
        }

        // -------------------------------------
        // 7. OBTENER URLS DE DESCARGA DEL CURSO
        // -------------------------------------
        [HttpGet("{cursoId}/descargas")]
        public async Task<IActionResult> ObtenerUrlsDescarga(
            [FromRoute] int cursoId,
            [FromQuery] int usuarioId,
            [FromQuery] int minutosExpiracion = 60)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (usuarioId != userId & userRole != "Administrador")
                return Forbid(); // No puedes modificar otro usuario

            var urls = await _cursoService.ObtenerUrlsDescargaCursoAsync(cursoId, usuarioId, minutosExpiracion, userRole);
            return Ok(urls.Select(u => u.ToString()));
        }

        // --------------------------------------
        // 8. OBTENER DETALLES DEL VIDEO
        // --------------------------------------
        [HttpGet("videos/{videoId}/stream")]
        public async Task<IActionResult> ObtenerVideoDetalleAsync(int videoId, int usuarioId, int minutosExpiracion)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (usuarioId != userId & userRole != "Administrador")
                return Forbid(); // No puedes modificar otro usuario

            var video = await _cursoService.ObtenerVideoDetalleAsync(videoId, usuarioId, minutosExpiracion, userRole);
            return Ok(video);
        }
    }
}
