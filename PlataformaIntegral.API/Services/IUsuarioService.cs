using PlataformaIntegral.API.DTOs;

namespace PlataformaIntegral.API.Services
{
    public interface IUsuarioService
    {
        /// <summary>
        /// Obtiene un usuario por su ID.
        /// </summary>
        Task<UsuarioReadDto?> GetByIdAsync(int id);

        /// <summary>
        /// Obtiene todos los usuarios.
        /// </summary>
        Task<IEnumerable<UsuarioReadDto>> GetAllAsync();

        /// <summary>
        /// Crea un nuevo usuario (flujo administrativo).
        /// </summary>
        Task<UsuarioReadDto> CreateAsync(UsuarioCreateDto dto);

        /// <summary>
        /// Actualiza un usuario existente (incluye imagen de perfil).
        /// </summary>
        Task<bool> UpdateAsync(int id, UsuarioUpdateDto dto);

        /// <summary>
        /// Elimina un usuario por su ID.
        /// </summary>
        Task<bool> DeleteAsync(int id);
    }
}
