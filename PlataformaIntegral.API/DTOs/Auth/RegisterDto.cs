namespace PlataformaIntegral.API.DTOs.Auth
{
    public class RegisterDto
    {
        public string Nombre { get; set; } = null!;
        public string Apellido { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Contrasena { get; set; } = null!;
        public int? IdTipoUsuario { get; set; }

        // Campos adicionales según el tipo de usuario
        public string? Educacion { get; set; }          // Estudiante
        public string? Disponibilidad { get; set; }     // Profesor
        public string? Rol { get; set; }                // Administrador
    }
}
