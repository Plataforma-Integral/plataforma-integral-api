using Microsoft.EntityFrameworkCore;
using PlataformaIntegral.API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlataformaIntegral.API.DTOs
{
    public class ResenaCursoCreateDto
    {
        public int IdEstudiante { get; set; }

        public int IdCurso { get; set; }

        public bool Opinion { get; set; }

        [StringLength(300)]
        [Unicode(false)]
        public string? Comentario { get; set; }
    }
}
