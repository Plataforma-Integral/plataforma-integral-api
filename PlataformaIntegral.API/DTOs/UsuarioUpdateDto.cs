using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace PlataformaIntegral.API.DTOs
{
    public class UsuarioUpdateDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(80, ErrorMessage = "El nombre no puede tener más de 80 caracteres")]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "El apellido es obligatorio")]
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

        public int? IdTipoUsuario { get; set; }
        public int? IdPais { get; set; }

        [StringLength(80)]
        public string? Departamento { get; set; }

        [StringLength(100)]
        public string? Ciudad { get; set; }

        [StringLength(100)]
        public string? NivelEducativo { get; set; }

        // Imagen de perfil (se sube a MinIO en el Update)
        public IFormFile? ImagenPerfil { get; set; }

        // Opcionales: credenciales y configuración de privacidad
        public CredencialCreateDto? Credencial { get; set; }
        public ConfigPrivacidadCreateDto? Config { get; set; }
    }
}
