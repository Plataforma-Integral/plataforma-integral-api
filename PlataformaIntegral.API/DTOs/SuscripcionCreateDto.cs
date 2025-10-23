using PlataformaIntegral.API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlataformaIntegral.API.DTOs
{
    public class SuscripcionCreateDto
    {
        public int? IdSuscripcionTipo { get; set; }

        public int? IdUsuario { get; set; }

        public DateOnly? FechaFin { get; set; }

        public int? IdEstadoSuscripcion { get; set; }
    }
}
