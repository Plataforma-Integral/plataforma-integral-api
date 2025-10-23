using PlataformaIntegral.API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlataformaIntegral.API.DTOs
{
    public class SuscripcionReadDto
    {
        public int IdSuscripcion { get; set; }

        public int? IdSuscripcionTipo { get; set; }

        public int? IdUsuario { get; set; }

        public DateOnly? FechaInicio { get; set; }

        public DateOnly? FechaFin { get; set; }

        public int? IdEstadoSuscripcion { get; set; }

        public string? NombreEstadoSuscripcion { get; set; }

        public string? NombreSuscripcionTipo { get; set; }

        public string? NombreUsuario { get; set; }
    }
}
