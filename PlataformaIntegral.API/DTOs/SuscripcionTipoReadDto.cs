using PlataformaIntegral.API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlataformaIntegral.API.DTOs
{
    public class SuscripcionTipoReadDto
    {
        public int IdProducto { get; set; }

        public int DuracionMeses { get; set; }

        public string? NombreProducto { get; set; } = null!;
    }
}
