using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlataformaIntegral.API.Models;

namespace PlataformaIntegral.API.Controllers
{
    // 🔹 La ruta base del controlador será: api/usuarios
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly PlataformaIntegralContext _context;

        // 🔹 Inyección del DbContext (ya configurado en Program.cs)
        public UsuariosController(PlataformaIntegralContext context)
        {
            _context = context;
        }

        // 🟢 GET: api/usuarios
        // Devuelve todos los usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            return await _context.Usuarios.ToListAsync();
        }

        // 🟢 GET: api/usuarios/5
        // Devuelve un usuario por su ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
                return NotFound(); // Devuelve 404 si no existe

            return usuario;
        }

        // 🟠 POST: api/usuarios
        // Crea un nuevo usuario
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            // Devuelve 201 Created con la ruta del nuevo recurso
            return CreatedAtAction(nameof(GetUsuario), new { id = usuario.IdUsuario }, usuario);
        }

        // 🟡 PUT: api/usuarios/5
        // Actualiza un usuario existente
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, Usuario usuario)
        {
            if (id != usuario.IdUsuario)
                return BadRequest();

            _context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Usuarios.Any(e => e.IdUsuario == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent(); // 204 sin contenido
        }

        // 🔴 DELETE: api/usuarios/5
        // Elimina un usuario
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return NotFound();

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
