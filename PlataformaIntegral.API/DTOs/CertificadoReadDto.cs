using PlataformaIntegral.API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlataformaIntegral.API.DTOs
{
    public class CertificadoReadDto
    {
        public int IdRecurso { get; set; }

        public int IdCurso { get; set; }

        public string NombreCurso { get; set; } = null!;

        public string NombreRecurso { get; set; } = null!;

    }
}
