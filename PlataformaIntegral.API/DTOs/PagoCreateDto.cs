using Microsoft.EntityFrameworkCore;
using PlataformaIntegral.API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlataformaIntegral.API.DTOs
{
    public class PagoCreateDto
    {
        public int? IdRecibo { get; set; }

        public int IdUsuario { get; set; }

        public int IdTipoMoneda { get; set; }

        public int IdProducto { get; set; }

        public int IdMetodoPago { get; set; }

        public int IdEstadoPago { get; set; }

        [StringLength(100)]
        [Unicode(false)]
        public string? ReferenciaTransaccion { get; set; }

        public decimal? Monto { get; set; }
    }
}
