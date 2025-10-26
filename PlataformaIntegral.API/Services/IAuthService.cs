namespace PlataformaIntegral.API.Services
{
    public interface IAuthService
    {
        Task<DTOs.Auth.AuthResponseDto?> LoginAsync(DTOs.Auth.LoginDto loginDto);
        Task<DTOs.Auth.AuthResponseDto> RegisterAsync(DTOs.Auth.RegisterDto registerDto);
        string GenerateJwtToken(string email, int idUsuario, string role);
    }
}
