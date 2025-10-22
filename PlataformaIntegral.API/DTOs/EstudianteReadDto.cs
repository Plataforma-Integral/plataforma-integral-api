using Microsoft.EntityFrameworkCore;
using PlataformaIntegral.API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlataformaIntegral.API.DTOs
{
    public class EstudianteReadDto
    {
        public int IdUsuario { get; set; }

        [StringLength(120)]
        [Unicode(false)]
        public string? Educacion { get; set; }

        public DateTime? UltimoLogin { get; set; }

        public int? Puntos { get; set; }

        public string NombreUsuario { get; set; } = null!;
    }
}
