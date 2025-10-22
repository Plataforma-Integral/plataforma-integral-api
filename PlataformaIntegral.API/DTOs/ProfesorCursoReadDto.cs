using PlataformaIntegral.API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlataformaIntegral.API.DTOs
{
    public class ProfesorCursoReadDto
    {
        public int IdProfesor { get; set; }

        public int IdCurso { get; set; }

        public int? IdTipoRol { get; set; }

        public string? NombreCurso { get; set; } = null!;

        public string? NombreProfesor { get; set; } = null!;

        public string? NombreTipoRol { get; set; }
    }
}
