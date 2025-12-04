using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlataformaIntegral.API.Models;

namespace PlataformaIntegral.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CheckDatabaseController : ControllerBase
    {
        private readonly PlataformaIntegralContext _context;

        public CheckDatabaseController(PlataformaIntegralContext context)
        {
            _context = context;
        }

        [HttpGet("check")]
        public IActionResult CheckDatabase()
        {
            bool canConnect = _context.Database.CanConnect();
            return Ok(new { connected = canConnect });
        }

        [HttpGet("test")]
        public IActionResult TestDb()
        {
            try
            {
                _context.Database.OpenConnection();
                _context.Database.CloseConnection();
                return Ok(new { message = "Conexión a la BD OK" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
