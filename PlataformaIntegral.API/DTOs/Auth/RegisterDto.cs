using System.ComponentModel.DataAnnotations;

namespace PlataformaIntegral.API.DTOs.Auth
{
    public class RegisterDto
    {
        [StringLength(80)]
        public string Nombre { get; set; } = null!;

        [StringLength(80)]
        public string Apellido { get; set; } = null!;

        [Required, EmailAddress]
        public string Email { get; set; } = null!;

        [Required, StringLength(100, MinimumLength = 6)]
        public string Contrasena { get; set; } = null!;

        public int? IdTipoUsuario { get; set; }

        // Campos adicionales según el tipo de usuario
        public string? NivelEducativo { get; set; }   // Estudiante
        public string? Disponibilidad { get; set; }   // Profesor
        public string? Rol { get; set; }              // Administrador
    }
}
