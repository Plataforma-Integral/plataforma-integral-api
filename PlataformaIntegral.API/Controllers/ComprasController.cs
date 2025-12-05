using Microsoft.AspNetCore.Mvc;
using PlataformaIntegral.API.DTOs;
using PlataformaIntegral.API.Services;
using PlataformaIntegral.API.Services.PlataformaIntegral.API.Services;

namespace PlataformaIntegral.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ComprasController : ControllerBase
    {
        private readonly ICompraService _comprasService;

        public ComprasController(ICompraService comprasService)
        {
            _comprasService = comprasService;
        }

        [HttpPost("curso")]
        public async Task<ActionResult<CompraResponseDto>> ComprarCurso([FromBody] CompraCursoDto dto)
        {
            var result = await _comprasService.ComprarCursoAsync(dto);
            return Ok(result);
        }
    }
}
