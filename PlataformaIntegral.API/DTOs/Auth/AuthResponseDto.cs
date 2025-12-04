namespace PlataformaIntegral.API.DTOs.Auth
{
    public class AuthResponseDto
    {
        public string? Token { get; set; } = null!;
        public DateTime Expiration { get; set; }
        public int IdUsuario { get; set; }
        public string Email { get; set; } = null!;
        public string Rol { get; set; } = null!;
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? ImagenUrl { get; set; }   // ✅ Nueva propiedad
        public bool Success { get; set; } = true;
        public string? Message { get; set; }
    }
}
