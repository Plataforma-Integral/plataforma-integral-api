using PlataformaIntegral.API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlataformaIntegral.API.DTOs
{
    public class ExamenCreateDto
    {
        public int IdRecurso { get; set; }

        public int? IdCursoPregrabado { get; set; }

        public int? PuntuacionMinima { get; set; }
    }
}
