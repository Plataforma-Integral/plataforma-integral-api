using Microsoft.EntityFrameworkCore;
using PlataformaIntegral.API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlataformaIntegral.API.DTOs
{
    public class CursoPregrabadoCreateDto
    {
        public int IdCurso { get; set; }
        public int? IdCertificado { get; set; }

        public int? PrecioPuntos { get; set; }

        [StringLength(300)]
        [Unicode(false)]
        public string? UrlPortada { get; set; }

        [StringLength(300)]
        [Unicode(false)]
        public string? UrlVideo { get; set; }
    }
}
