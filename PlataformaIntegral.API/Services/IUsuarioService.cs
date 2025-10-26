using PlataformaIntegral.API.DTOs;

namespace PlataformaIntegral.API.Services
{
    public interface IUsuarioService
    {
        Task<UsuarioReadDto?> GetByIdAsync(int id);
        Task<IEnumerable<UsuarioReadDto>> GetAllAsync();
        Task<bool> UpdateAsync(int id, UsuarioCreateDto dto);
    }
}
