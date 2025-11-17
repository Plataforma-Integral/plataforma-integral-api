using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlataformaIntegral.API.Models;

namespace PlataformaIntegral.API.Controllers
{
    public class CheckDatabaseController : ControllerBase
    {
        private readonly PlataformaIntegralContext _context;

        // 🔹 Inyección del DbContext (ya configurado en Program.cs)
        public CheckDatabaseController(PlataformaIntegralContext context)
        {
            _context = context;
        }

        [HttpGet("check-db")]
        public IActionResult CheckDatabase()
        {
            bool canConnect = _context.Database.CanConnect();
            return Ok(new { connected = canConnect });
        }
        [HttpGet("test-db")]
        public IActionResult TestDb([FromServices] PlataformaIntegralContext context)
        {
            try
            {
                context.Database.OpenConnection();
                context.Database.CloseConnection();
                return Ok("Conexión a la BD OK");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
