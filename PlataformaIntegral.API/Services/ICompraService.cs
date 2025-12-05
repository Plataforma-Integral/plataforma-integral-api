using PlataformaIntegral.API.DTOs;

namespace PlataformaIntegral.API.Services
{
    public interface ICompraService
    {
        Task<CompraResponseDto> ComprarCursoAsync(CompraCursoDto dto);
    }
}
