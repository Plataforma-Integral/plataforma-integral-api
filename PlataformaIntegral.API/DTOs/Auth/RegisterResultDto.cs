namespace PlataformaIntegral.API.DTOs.Auth
{
    public class RegisterResultDto
    {
        public int IdUsuario { get; set; }
        public string Email { get; set; } = null!;
        public string Rol { get; set; } = "Estudiante"; // Por defecto
    }
}
