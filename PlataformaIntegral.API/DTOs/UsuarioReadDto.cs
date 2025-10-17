using Microsoft.EntityFrameworkCore;
using PlataformaIntegral.API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        public int? IdTipoUsuario { get; set; }

        public int? IdPais { get; set; }

        [StringLength(80)]
        public string? Departamento { get; set; }

        [StringLength(100)]
        public string? Ciudad { get; set; }

        [StringLength(100)]
        public string? NivelEducativo { get; set; }

        public DateTime? FechaRegistro { get; set; }

        public ConfiguracionPrivacidad? ConfiguracionPrivacidad { get; set; }

        public Estudiante? Estudiante { get; set; }
        public Profesor? Profesor { get; set; }
        public Administrador? Administrador { get; set; }
        public Pais? IdPaisNavigation { get; set; }
    }
}
