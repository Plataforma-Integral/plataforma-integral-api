using Microsoft.AspNetCore.Mvc;
using PlataformaIntegral.API.DTOs;
using PlataformaIntegral.API.Services;

namespace PlataformaIntegral.API.Controllers
{
    [ApiController]
    [Route("api/v1/catalogos")]
    public class CatalogosController : ControllerBase
    {
        private readonly ICatalogoService _catalogoService;

        public CatalogosController(ICatalogoService catalogoService)
        {
            _catalogoService = catalogoService;
        }

        [HttpGet("paises")]
        public async Task<ActionResult<IEnumerable<CatalogoDto>>> GetPaises()
        {
            return Ok(await _catalogoService.ObtenerPaisesAsync());
        }

        [HttpGet("tipos-usuario")]
        public async Task<ActionResult<IEnumerable<CatalogoDto>>> GetTiposUsuario()
        {
            return Ok(await _catalogoService.ObtenerTiposUsuarioAsync());
        }

        [HttpGet("niveles-medalla")]
        public async Task<ActionResult<IEnumerable<CatalogoDto>>> GetNivelesMedalla()
        {
            return Ok(await _catalogoService.ObtenerNivelesMedallaAsync());
        }

        [HttpGet("modalidades-sincronico")]
        public async Task<ActionResult<IEnumerable<CatalogoDto>>> GetModalidadesSincronico()
        {
            return Ok(await _catalogoService.ObtenerModalidadesSincronicoAsync());
        }

        [HttpGet("roles")]
        public async Task<ActionResult<IEnumerable<CatalogoDto>>> GetTiposRol()
        {
            return Ok(await _catalogoService.ObtenerTiposRolAsync());
        }

        [HttpGet("estados-suscripcion")]
        public async Task<ActionResult<IEnumerable<CatalogoDto>>> GetEstadosSuscripcion()
        {
            return Ok(await _catalogoService.ObtenerEstadosSuscripcionAsync());
        }

        [HttpGet("estados-pago")]
        public async Task<ActionResult<IEnumerable<CatalogoDto>>> GetEstadosPago()
        {
            return Ok(await _catalogoService.ObtenerEstadosPagoAsync());
        }

        [HttpGet("metodos-pago")]
        public async Task<ActionResult<IEnumerable<CatalogoDto>>> GetMetodosPago()
        {
            return Ok(await _catalogoService.ObtenerMetodosPagoAsync());
        }

        [HttpGet("monedas")]
        public async Task<ActionResult<IEnumerable<CatalogoDto>>> GetTiposMoneda()
        {
            return Ok(await _catalogoService.ObtenerTiposMonedaAsync());
        }
    }
}
