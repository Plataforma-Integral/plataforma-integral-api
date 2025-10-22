using PlataformaIntegral.API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlataformaIntegral.API.DTOs
{
    public class CursoSincronicoReadDto
    {
        public int IdCurso { get; set; }

        public int? IdModalidad { get; set; }

        public DateOnly? FechaInicio { get; set; }

        public int? Duracion { get; set; }

        public string? NombreCurso { get; set; } = null!;

        public string? NombreModalidad { get; set; }
    }
}
