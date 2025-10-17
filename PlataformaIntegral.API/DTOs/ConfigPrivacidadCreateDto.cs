using PlataformaIntegral.API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlataformaIntegral.API.DTOs
{
    public class ConfigPrivacidadCreateDto
    {
        [Required]
        public int IdUsuario { get; set; }

        public bool? MostrarEmail { get; set; }

        public bool? MostrarTelefono { get; set; }

        public bool? MostrarNombre { get; set; }

        public bool? MostrarFechaNacimiento { get; set; }

        public bool? MostrarMedallas { get; set; }
    }
}
