using Microsoft.EntityFrameworkCore;
using PlataformaIntegral.API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlataformaIntegral.API.DTOs
{
    public class CapituloReadDto
    {
        public int IdCapitulo { get; set; }

        public int IdCursoPregrabado { get; set; }

        [StringLength(120)]
        [Unicode(false)]
        public string? Nombre { get; set; }

        [StringLength(400)]
        [Unicode(false)]
        public string? Descripcion { get; set; }

        public int? NumeroOrden { get; set; }

        //public virtual ICollection<Cuestionario> Cuestionarios { get; set; } = new List<Cuestionario>();

        //public virtual CursoPregrabado CursoPregrabado { get; set; } = null!;

        //public virtual ICollection<Video> Videos { get; set; } = new List<Video>();
    }
}
