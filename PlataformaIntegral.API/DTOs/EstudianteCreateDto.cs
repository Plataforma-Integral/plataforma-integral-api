using Microsoft.EntityFrameworkCore;
using PlataformaIntegral.API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlataformaIntegral.API.DTOs
{
    public class EstudianteCreateDto
    {
        public int IdUsuario { get; set; }

        [StringLength(120)]
        [Unicode(false)]
        public string? Educacion { get; set; }

        public int? Puntos { get; set; }

        public virtual string? NombreUsuario { get; set; } = null!;
    }
}
