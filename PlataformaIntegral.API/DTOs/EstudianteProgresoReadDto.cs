using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace PlataformaIntegral.API.DTOs
{
    public class EstudianteProgresoReadDto
    {
        public int IdEstudiante { get; set; }

        public int IdRecurso { get; set; }

        [StringLength(50)]
        [Unicode(false)]
        public string? Estado { get; set; }

        public string? NombreEstudiante { get; set; } = null!;

        public string? NombreRecurso { get; set; } = null!;
    }
}
