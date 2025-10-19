using PlataformaIntegral.API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlataformaIntegral.API.DTOs
{
    public class CertificadoCreateDto
    {
        [Required]
        public int IdCurso { get; set; }

    }
}
