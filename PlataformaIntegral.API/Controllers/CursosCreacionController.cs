using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlataformaIntegral.API.DTOs;
using PlataformaIntegral.API.Services;

namespace PlataformaIntegral.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize(Roles = "Profesor, Administrador")]
    public class CursosCreacionController : ControllerBase
    {
        private readonly ICrearCursoService _crearCursoService;
        private readonly ILogger<CursosCreacionController> _logger;
        private readonly ICursoService _cursoService;

        public CursosCreacionController(ICrearCursoService crearCursoService,
                                ICursoService cursoService,
                                ILogger<CursosCreacionController> logger)
        {
            _crearCursoService = crearCursoService;
            _cursoService = cursoService;
            _logger = logger;
        }

        // -------------------------------------
        // 1. Crear curso pregrabado en borrador
        // -------------------------------------
        [HttpPost("pregrabados")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CrearCursoPregrabado([FromForm] CursoPregrabadoDto dto)
        {
            if (dto == null)
                return BadRequest("Datos del curso inválidos.");

            try
            {
                int cursoId = await _crearCursoService.CrearCursoPregrabadoAsync(dto);
                var cursoCard = await _cursoService.ObtenerCursoCardPorIdAsync(cursoId);

                return Ok(cursoCard);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando curso pregrabado.");
                return StatusCode(500, "Error interno al crear el curso.");
            }
        }

        [HttpPost("pregrabados/{cursoId}/capitulos")]
        public async Task<IActionResult> AgregarCapitulo([FromRoute] int cursoId, [FromBody] CapituloCreateDto capitulo)
        {
            if (string.IsNullOrWhiteSpace(capitulo.Nombre))
                return BadRequest("El nombre del capítulo no puede estar vacío.");

            try
            {
                // Usar cursoId de la ruta, no del body (evita redundancia)
                int capituloId = await _crearCursoService.AgregarCapituloAsync(cursoId, capitulo.Nombre);
                return Ok(new { CapituloId = capituloId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error agregando capítulo.");
                return StatusCode(500, "Error interno al agregar capítulo.");
            }
        }

        // -------------------------------------
        // 3. Subir video a un capítulo
        // -------------------------------------
        [HttpPost("capitulos/{capituloId}/videos")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> SubirVideo([FromRoute] int capituloId, [FromForm] VideoDto dto)
        {
            if (dto?.Archivo == null)
                return BadRequest("Debes proporcionar un archivo.");

            try
            {
                int videoId = await _crearCursoService.SubirVideoAsync(capituloId, dto);
                return Ok(new { VideoId = videoId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error subiendo video.");
                return StatusCode(500, "Error interno al subir video.");
            }
        }

        // -------------------------------------
        // 4. Publicar curso
        // -------------------------------------
        [HttpPut("pregrabados/{cursoId}/publicar")]
        public async Task<IActionResult> PublicarCurso([FromRoute] int cursoId)
        {
            try
            {
                await _crearCursoService.PublicarCursoAsync(cursoId);
                return Ok(new { Mensaje = "Curso publicado exitosamente." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error publicando curso.");
                return StatusCode(500, "Error interno al publicar curso.");
            }
        }

        // -------------------------------------
        // 5. Modificar curso pregrabado
        // -------------------------------------
        [HttpPut("pregrabados/{cursoId}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> ModificarCursoPregrabado([FromRoute] int cursoId, [FromForm] CursoPregrabadoDto dto)
        {
            if (dto == null)
                return BadRequest("Datos del curso inválidos.");

            try
            {
                await _crearCursoService.ModificarCursoPregrabadoAsync(cursoId, dto);
                return Ok(new { Mensaje = "Curso modificado exitosamente." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error modificando curso.");
                return StatusCode(500, "Error interno al modificar curso.");
            }
        }

        // -------------------------------------
        // 6. Modificar capítulo
        // -------------------------------------
        [HttpPut("capitulos/{capituloId}")]
        public async Task<IActionResult> ModificarCapitulo([FromRoute] int capituloId, [FromBody] string nuevoNombre)
        {
            if (string.IsNullOrWhiteSpace(nuevoNombre))
                return BadRequest("El nombre del capítulo no puede estar vacío.");

            try
            {
                await _crearCursoService.ModificarCapituloAsync(capituloId, nuevoNombre);
                return Ok(new { Mensaje = "Capítulo modificado exitosamente." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error modificando capítulo.");
                return StatusCode(500, "Error interno al modificar capítulo.");
            }
        }

        // -------------------------------------
        // 7. Modificar video
        // -------------------------------------
        [HttpPut("videos/{videoId}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> ModificarVideo([FromRoute] int videoId, [FromForm] VideoDto dto)
        {
            if (dto == null)
                return BadRequest("Datos del video inválidos.");

            try
            {
                await _crearCursoService.ModificarVideoAsync(videoId, dto);
                return Ok(new { Mensaje = "Video modificado exitosamente." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error modificando video.");
                return StatusCode(500, "Error interno al modificar video.");
            }
        }

        // -------------------------------------
        // 8. Eliminar capítulo
        // -------------------------------------
        [HttpDelete("capitulos/{capituloId}")]
        public async Task<IActionResult> EliminarCapitulo([FromRoute] int capituloId)
        {
            try
            {
                await _crearCursoService.EliminarCapituloAsync(capituloId);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminando capítulo.");
                return StatusCode(500, "Error interno al eliminar capítulo.");
            }
        }

        // -------------------------------------
        // 9. Eliminar video
        // -------------------------------------
        [HttpDelete("videos/{videoId}")]
        public async Task<IActionResult> EliminarVideo([FromRoute] int videoId)
        {
            try
            {
                await _crearCursoService.EliminarVideoAsync(videoId);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminando video.");
                return StatusCode(500, "Error interno al eliminar video.");
            }
        }

        // -------------------------------------
        // 10. Eliminar curso
        // -------------------------------------
        [HttpDelete("pregrabados/{cursoId}")]
        public async Task<IActionResult> EliminarCurso([FromRoute] int cursoId)
        {
            try
            {
                await _crearCursoService.EliminarCursoAsync(cursoId);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminando curso.");
                return StatusCode(500, "Error interno al eliminar curso.");
            }
        }
    }
}
