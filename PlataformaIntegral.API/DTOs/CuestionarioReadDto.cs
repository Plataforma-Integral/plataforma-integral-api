using PlataformaIntegral.API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlataformaIntegral.API.DTOs
{
    public class CuestionarioReadDto
    {
        public int IdRecurso { get; set; }

        public int IdCapitulo { get; set; }

        public int? NumeroOrden { get; set; }

        public string? NombreCapitulo { get; set; } = null!;

        public string? NombreRecurso { get; set; } = null!;
    }
}
