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

    [HttpGet]
    public async Task<IActionResult> GetUsuarios()
    {
        var result = await _service.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUsuario(int id)
    {
        var result = await _service.GetByIdAsync(id);
        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUsuario(int id, UsuarioCreateDto dto)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

        if (id != userId & userRole != "Administrador")
            return Forbid(); // No puedes modificar otro usuario

        var success = await _service.UpdateAsync(id, dto);

        return success ? Ok() : NotFound();
    }
}
