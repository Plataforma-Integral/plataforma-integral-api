using PlataformaIntegral.API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlataformaIntegral.API.DTOs
{
    public class SuscripcionTipoCreateDto
    {
        public int IdProducto { get; set; }

        public int DuracionMeses { get; set; }
    }
}
