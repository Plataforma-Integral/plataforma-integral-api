using Microsoft.EntityFrameworkCore;
using PlataformaIntegral.API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlataformaIntegral.API.DTOs
{
    public class CapituloCreateDto
    {
        [Required]
        public int IdCursoPregrabado { get; set; }

        [StringLength(120)]
        [Unicode(false)]
        [Required]
        public string? Nombre { get; set; }

        [StringLength(400)]
        [Unicode(false)]
        public string? Descripcion { get; set; }
        [Required]
        public int? NumeroOrden { get; set; }

    }
}
