using Microsoft.EntityFrameworkCore;
using PlataformaIntegral.API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlataformaIntegral.API.DTOs
{
    public class ResenaProfesorReadDto
    {
        public int IdEstudiante { get; set; }

        public int IdProfesor { get; set; }

        public bool Opinion { get; set; }

        [StringLength(300)]
        [Unicode(false)]
        public string? Comentario { get; set; }

        public string? NombreEstudiante { get; set; } = null!;

        public string? NombreProfesor { get; set; } = null!;
    }
}
