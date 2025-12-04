using System.ComponentModel.DataAnnotations;

namespace PlataformaIntegral.API.DTOs
{
    public class UsuarioReadDto
    {
        public int IdUsuario { get; set; }

        [StringLength(80)]
        public string Nombre { get; set; } = null!;

        [StringLength(80)]
        public string Apellido { get; set; } = null!;

        [StringLength(50)]
        public string? Seudonimo { get; set; }

        [StringLength(400)]
        public string? Biografia { get; set; }

        [StringLength(20)]
        public string? Telefono { get; set; }

        [StringLength(1)]
        public string? Genero { get; set; }

        public DateOnly? FechaNacimiento { get; set; }

        public string? TipoUsuario { get; set; }

        public string? Pais { get; set; }

        [StringLength(80)]
        public string? Departamento { get; set; }

        [StringLength(100)]
        public string? Ciudad { get; set; }

        [StringLength(100)]
        public string? NivelEducativo { get; set; }

        public DateTime? FechaRegistro { get; set; }

        public ConfigPrivacidadReadDto? ConfiguracionPrivacidad { get; set; }

        /// <summary>
        /// URL pública (presignada) para acceder a la imagen de perfil.
        /// </summary>
        public string? ImagenUrl { get; set; }

        // Nota: No incluir la propiedad Credencial para evitar exponer contraseñas
    }
}
