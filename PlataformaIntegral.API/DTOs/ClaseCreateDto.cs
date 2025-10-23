using Microsoft.EntityFrameworkCore;
using PlataformaIntegral.API.Enums;
using PlataformaIntegral.API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlataformaIntegral.API.DTOs
{
    public class ClaseCreateDto
    {
        [Required]
        public int IdProfesor { get; set; }

        [Required]
        public int IdCursoSincronico { get; set; }

        [Required]
        [StringLength(120)]
        [Unicode(false)]
        public string? Nombre { get; set; }

        [StringLength(400)]
        [Unicode(false)]
        public string? Descripcion { get; set; }

        public DateOnly? Fecha { get; set; }

        public TimeOnly? Hora { get; set; }
        
        public TipoClaseEnum TipoClase { get; set; }

        public string? DireccionClasePresencial { get; set; } 

        public string? UrlClaseVirtual { get; set; }
    }
}
