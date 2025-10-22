using Microsoft.EntityFrameworkCore;
using PlataformaIntegral.API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlataformaIntegral.API.DTOs
{
    public class PagoReadDto
    {
        public int IdPago { get; set; }

        public int? IdRecibo { get; set; }

        public int IdUsuario { get; set; }

        public int IdTipoMoneda { get; set; }

        public int IdProducto { get; set; }

        public int IdMetodoPago { get; set; }

        public int IdEstadoPago { get; set; }

        public DateOnly? FechaPago { get; set; }

        [StringLength(100)]
        [Unicode(false)]
        public string? ReferenciaTransaccion { get; set; }

        public decimal? Monto { get; set; }

        public string? NombreEstadoPago { get; set; } = null!;

        public string? NombreMetodoPago { get; set; } = null!;

        public string? NombreProducto { get; set; } = null!;

        public string? NumeroRecibo { get; set; }

        public string? NombreTipoMoneda { get; set; } = null!;

        public string? NombreUsuario { get; set; } = null!;
    }
}
