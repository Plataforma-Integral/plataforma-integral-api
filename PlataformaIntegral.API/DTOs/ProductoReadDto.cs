using Microsoft.EntityFrameworkCore;
using PlataformaIntegral.API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlataformaIntegral.API.DTOs
{
    public class ProductoReadDto
    {
        public int IdProducto { get; set; }

        [StringLength(120)]
        [Unicode(false)]
        public string Nombre { get; set; } = null!;

        [StringLength(400)]
        [Unicode(false)]
        public string? Descripcion { get; set; }

        public decimal Precio { get; set; }

        public DateTime? FechaCreacion { get; set; }
    }
}
