using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlataformaIntegral.API.DTOs;
using PlataformaIntegral.API.Services;

namespace PlataformaIntegral.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CursosCreacionController : ControllerBase
    {
        private readonly ICrearCursoService _crearCursoService;

        public CursosCreacionController(ICrearCursoService crearCursoService)
        {
            _crearCursoService = crearCursoService;
        }

        // -------------------------------------
        // 1. Crear curso pregrabado en borrador
        // -------------------------------------
        [HttpPost("pregrabado")]
        public async Task<IActionResult> CrearCursoPregrabado([FromForm] CursoPregrabadoDto dto)
        {
            if (dto == null)
                return BadRequest("Datos del curso inválidos.");

            int cursoId = await _crearCursoService.CrearCursoPregrabadoAsync(dto);
            return Ok(new { CursoId = cursoId });
        }

        // -------------------------------------
        // 2. Agregar capítulo a un curso
        // -------------------------------------
        [HttpPost("{cursoId}/capitulo")]
        public async Task<IActionResult> AgregarCapitulo([FromRoute] int cursoId, [FromBody] string nombreCapitulo)
        {
            if (string.IsNullOrWhiteSpace(nombreCapitulo))
                return BadRequest("El nombre del capítulo no puede estar vacío.");

            int capituloId = await _crearCursoService.AgregarCapituloAsync(cursoId, nombreCapitulo);
            return Ok(new { CapituloId = capituloId });
        }

        // -------------------------------------
        // 3. Subir video a un capítulo
        // -------------------------------------
        [HttpPost("capitulo/{capituloId}/video")]
        public async Task<IActionResult> SubirVideo([FromRoute] int capituloId, [FromForm] VideoDto dto)
        {
            if (dto == null || (dto.Archivo == null))
                return BadRequest("Debes proporcionar un archivo");

            int videoId = await _crearCursoService.SubirVideoAsync(capituloId, dto);
            return Ok(new { VideoId = videoId });
        }

        // -------------------------------------
        // 4. Publicar curso
        // -------------------------------------
        [HttpPost("{cursoId}/publicar")]
        public async Task<IActionResult> PublicarCurso([FromRoute] int cursoId)
        {
            await _crearCursoService.PublicarCursoAsync(cursoId);
            return Ok(new { Mensaje = "Curso publicado exitosamente." });
        }

        // -------------------------------------
        // Más métodos para edición y eliminación pueden añadirse aquí
        // -------------------------------------

        [HttpPut("{cursoId}/Modificar")]
        public async Task<IActionResult> ModificarCursoPregrabado([FromRoute] int cursoId, [FromForm] CursoPregrabadoDto dto)
        {
            if (dto == null)
                return BadRequest("Datos del curso inválidos.");
            await _crearCursoService.ModificarCursoPregrabadoAsync(cursoId, dto);
            return Ok(new { Mensaje = "Curso modificado exitosamente." });
        }

        [HttpPut("capitulo/{capituloId}/Modificar")]
        public async Task<IActionResult> ModificarCapitulo([FromRoute] int capituloId, [FromBody] string nuevoNombre)
        {
            if (string.IsNullOrWhiteSpace(nuevoNombre))
                return BadRequest("El nombre del capítulo no puede estar vacío.");
            await _crearCursoService.ModificarCapituloAsync(capituloId, nuevoNombre);
            return Ok(new { Mensaje = "Capítulo modificado exitosamente." });
        }

        [HttpPut("video/{videoId}/Modificar")]
        public async Task<IActionResult> ModificarVideo([FromRoute] int videoId, [FromForm] VideoDto dto)
        {
            if (dto == null)
                return BadRequest("Datos del video inválidos.");
            await _crearCursoService.ModificarVideoAsync(videoId, dto);
            return Ok(new { Mensaje = "Video modificado exitosamente." });
        }

        [HttpDelete("capitulo/{capituloId}/Eliminar")]
        public async Task<IActionResult> EliminarCapitulo([FromRoute] int capituloId)
        {
            await _crearCursoService.EliminarCapituloAsync(capituloId);
            return Ok(new { Mensaje = "Capítulo eliminado exitosamente." });
        }

        [HttpDelete("video/{videoId}/Eliminar")]
        public async Task<IActionResult> EliminarVideo([FromRoute] int videoId)
        {
            await _crearCursoService.EliminarVideoAsync(videoId);
            return Ok(new { Mensaje = "Video eliminado exitosamente." });
        }

        [HttpDelete("{cursoId}/Eliminar")]
        public async Task<IActionResult> EliminarCurso([FromRoute] int cursoId)
        {
            await _crearCursoService.EliminarCursoAsync(cursoId);
            return Ok(new { Mensaje = "Curso eliminado exitosamente." });
        }
    }
}
