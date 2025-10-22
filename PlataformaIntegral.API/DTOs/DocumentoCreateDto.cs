using PlataformaIntegral.API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlataformaIntegral.API.DTOs
{
    public class DocumentoCreateDto
    {
        public int IdRecurso { get; set; }

        public int? IdVideo { get; set; }
    }
}
