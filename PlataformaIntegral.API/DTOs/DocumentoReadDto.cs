using PlataformaIntegral.API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlataformaIntegral.API.DTOs
{
    public class DocumentoReadDto
    {
        public int IdRecurso { get; set; }

        public int? IdVideo { get; set; }

        public string? NombreRecurso { get; set; } = null!;

        public string? NombreVideo { get; set; }
    }
}
