using Microsoft.EntityFrameworkCore;
using PlataformaIntegral.API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlataformaIntegral.API.DTOs
{
    public class EstudianteProgresoCreateDto
    {
        public int IdEstudiante { get; set; }

        public int IdRecurso { get; set; }

        [StringLength(50)]
        [Unicode(false)]
        public string? Estado { get; set; }
    }
}
