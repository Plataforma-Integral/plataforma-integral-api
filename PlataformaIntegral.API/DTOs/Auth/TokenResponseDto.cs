namespace PlataformaIntegral.API.DTOs.Auth
{
    public class TokenResponseDto
    {
        public string AccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
        public DateTime AccessTokenExpiresAt { get; set; }

    }
}
