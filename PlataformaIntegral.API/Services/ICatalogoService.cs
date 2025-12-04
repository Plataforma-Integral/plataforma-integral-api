using PlataformaIntegral.API.DTOs;

namespace PlataformaIntegral.API.Services
{
    public interface ICatalogoService
    {
        Task<List<CatalogoDto>> ObtenerPaisesAsync();
        Task<List<CatalogoDto>> ObtenerTiposUsuarioAsync();
        Task<List<CatalogoDto>> ObtenerNivelesMedallaAsync();
        Task<List<CatalogoDto>> ObtenerModalidadesSincronicoAsync();
        Task<List<CatalogoDto>> ObtenerTiposRolAsync();
        Task<List<CatalogoDto>> ObtenerEstadosSuscripcionAsync();
        Task<List<CatalogoDto>> ObtenerEstadosPagoAsync();
        Task<List<CatalogoDto>> ObtenerMetodosPagoAsync();
        Task<List<CatalogoDto>> ObtenerTiposMonedaAsync();
    }
}
