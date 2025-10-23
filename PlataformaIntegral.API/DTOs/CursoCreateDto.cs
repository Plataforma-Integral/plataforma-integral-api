using PlataformaIntegral.API.Enums;
using PlataformaIntegral.API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlataformaIntegral.API.DTOs
{
    public class CursoCreateDto
    {
        public int IdProducto { get; set; }

        public EstadoCursoEnum Estado { get; set; }
        
        public bool EsPrivado { get; set; } = true;

        [Required]
        public TipoCursoEnum TipoCurso { get; set; }

        public CursoPregrabadoCreateDto? CursoPregrabado { get; set; }

        public CursoSincronicoCreateDto? CursoSincronico { get; set; }
    }
}
