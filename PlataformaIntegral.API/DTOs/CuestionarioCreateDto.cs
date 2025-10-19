using PlataformaIntegral.API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlataformaIntegral.API.DTOs
{
    public class CuestionarioCreateDto
    {
        [Required]
        public int IdRecurso { get; set; }

        [Required]
        public int IdCapitulo { get; set; }

        [Required]
        public int? NumeroOrden { get; set; }

        public string? NombreCapitulo { get; set; } = null!;

        public string? NombreRecurso { get; set; } = null!;
    }
}
