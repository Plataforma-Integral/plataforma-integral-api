using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlataformaIntegral.API.DTOs;
using PlataformaIntegral.API.Services;
using System.Security.Claims;

[Route("api/[controller]")]
[ApiController]
[Authorize] // 🔒 Requiere JWT
public class UsuariosController : ControllerBase
{
    private readonly IUsuarioService _service;

    public UsuariosController(IUsuarioService service)
    {
        _service = service;
    }

    // =========================================
    // GET: api/usuarios
    // =========================================
    [HttpGet]
    public async Task<IActionResult> GetUsuarios()
    {
        var result = await _service.GetAllAsync();
        return Ok(result);
    }

    // =========================================
    // GET: api/usuarios/{id}
    // =========================================
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUsuario(int id)
    {
        var result = await _service.GetByIdAsync(id);
        if (result == null)
            return NotFound();

        return Ok(result);
    }

    // =========================================
    // POST: api/usuarios
    // =========================================
    [HttpPost]
    [Authorize(Roles = "Administrador")] // Solo admin puede crear usuarios
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> CreateUsuario([FromForm] UsuarioCreateDto dto)
    {
        var usuario = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetUsuario), new { id = usuario.IdUsuario }, usuario);
    }

    // =========================================
    // PUT: api/usuarios/{id}
    // =========================================
    [HttpPut("{id}")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UpdateUsuario(int id, [FromForm] UsuarioUpdateDto dto)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

        if (id != userId && userRole != "Administrador")
            return Forbid();

        var success = await _service.UpdateAsync(id, dto);

        return success
            ? Ok(new { success = true, message = "Usuario actualizado correctamente." })
            : NotFound(new { success = false, message = "Usuario no encontrado." });
    }

    // =========================================
    // DELETE: api/usuarios/{id}
    // =========================================
    [HttpDelete("{id}")]
    [Authorize(Roles = "Administrador")] // Solo admin puede eliminar usuarios
    public async Task<IActionResult> DeleteUsuario(int id)
    {
        var success = await _service.DeleteAsync(id);
        return success ? NoContent() : NotFound();
    }
}
