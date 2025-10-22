using PlataformaIntegral.API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlataformaIntegral.API.DTOs
{
    public class CursoSincronicoCreateDto
    {
        public int IdCurso { get; set; }

        public int? IdModalidad { get; set; }

        public DateOnly? FechaInicio { get; set; }

        public int? Duracion { get; set; }
    }
}
