using Microsoft.EntityFrameworkCore;
using PlataformaIntegral.API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlataformaIntegral.API.DTOs
{
    public class ProfesorCreateDto
    {
        public int IdUsuario { get; set; }

        public decimal? Calificacion { get; set; }

        public bool? EstadoVerificacion { get; set; }

        [StringLength(120)]
        [Unicode(false)]
        public string? Disponibilidad { get; set; }
    }
}
