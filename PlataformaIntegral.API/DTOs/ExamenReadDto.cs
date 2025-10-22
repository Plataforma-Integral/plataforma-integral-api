using PlataformaIntegral.API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlataformaIntegral.API.DTOs
{
    public class ExamenReadDto
    {
        public int IdRecurso { get; set; }

        public int? IdCursoPregrabado { get; set; }

        public int? PuntuacionMinima { get; set; }

        public virtual string? NombreCursoPregrabado { get; set; }

        public virtual string? NombreRecurso { get; set; } = null!;
    }
}
