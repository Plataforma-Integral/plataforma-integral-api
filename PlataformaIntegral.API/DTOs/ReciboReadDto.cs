using Microsoft.EntityFrameworkCore;
using PlataformaIntegral.API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlataformaIntegral.API.DTOs
{
    public class ReciboReadDto
    {
        public int IdRecibo { get; set; }

        public int IdUsuario { get; set; }

        public DateOnly? FechaEmision { get; set; }

        [StringLength(50)]
        [Unicode(false)]
        public string? NumeroRecibo { get; set; }

        public decimal? MontoTotal { get; set; }

        [StringLength(400)]
        [Unicode(false)]
        public string? Detalle { get; set; }

        public int? IdEstadoPago { get; set; }

        public string? NombreEstadoPago { get; set; }

        public string? NombreUsuario { get; set; } = null!;
    }
}
