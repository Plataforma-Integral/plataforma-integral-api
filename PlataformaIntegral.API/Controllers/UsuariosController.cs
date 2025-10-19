using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlataformaIntegral.API.DTOs;
using PlataformaIntegral.API.Models;

namespace PlataformaIntegral.API.Controllers
{
    // 🔹 La ruta base del controlador será: api/usuarios
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly PlataformaIntegralContext _context;
        private readonly IMapper _mapper;

        // 🔹 Inyección del DbContext (ya configurado en Program.cs)
        public UsuariosController(PlataformaIntegralContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // 🟢 GET: api/usuarios
        // Devuelve todos los usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioReadDto>>> GetUsuarios()
        {
            var usuarios = await _context.Usuarios.ToListAsync();
            var dtos = _mapper.Map<List<UsuarioReadDto>>(usuarios);
            return Ok(dtos);
        }

        // 🟢 GET: api/usuarios/5
        // Devuelve un usuario por su ID
        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioReadDto>> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
                return NotFound(); // Devuelve 404 si no existe

            var dto = _mapper.Map<UsuarioReadDto>(usuario);

            return Ok(dto); // Devuelve 200 OK con el DTO
        }

        // 🟠 POST: api/usuarios
        // Crea un nuevo usuario
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(UsuarioCreateDto dto)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync(); // 1️⃣ Inicia transacción

            try
            {
                // 2️⃣ Crea Usuario principal
                var usuario = _mapper.Map<Usuario>(dto);
                usuario.FechaRegistro = DateTime.Now;

                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                // 3️⃣ Crea Credencial y la asocia al usuario
                var credencial = _mapper.Map<Credencial>(dto.Credencial);
                credencial.IdUsuario = usuario.IdUsuario;
                _context.Credenciales.Add(credencial);

                // 4️⃣ Crea Configuración de Privacidad
                var config = _mapper.Map<ConfiguracionPrivacidad>(dto.Config);
                config.IdUsuario = usuario.IdUsuario;
                _context.ConfiguracionesPrivacidad.Add(config);

                // 5️⃣ Dependiendo del tipo, crea un Estudiante o un Profesor
                switch(dto.IdTipoUsuario)
                {
                    case null:
                        throw new Exception("El tipo de usuario es obligatorio");
                    case < 1 or > 3:
                        throw new Exception("Tipo de usuario inválido");
                    case 1:
                        _context.Estudiantes.Add(new Estudiante { IdUsuario = usuario.IdUsuario });
                        break;
                    case 2:
                        _context.Profesores.Add(new Profesor { IdUsuario = usuario.IdUsuario });
                        break;
                    case 3:
                        _context.Administradores.Add(new Administrador { IdUsuario = usuario.IdUsuario });
                        break;
                }

                // 6️⃣ Guarda todo
                await _context.SaveChangesAsync();
                await transaction.CommitAsync(); // 7️⃣ Confirma la transacción

                // 👇 Aquí el cambio: devolvemos el DTO, no la entidad
                var usuarioRead = _mapper.Map<UsuarioReadDto>(usuario);
                return CreatedAtAction(nameof(GetUsuario), new { id = usuario.IdUsuario }, usuarioRead);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(); // ❌ Si falla algo, deshace todo
                return BadRequest(new { message = "Error al crear el usuario", error = ex.Message, inner = ex.InnerException?.Message });
            }
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
