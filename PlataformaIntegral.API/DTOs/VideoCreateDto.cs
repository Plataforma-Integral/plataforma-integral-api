using Microsoft.EntityFrameworkCore;
using PlataformaIntegral.API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlataformaIntegral.API.DTOs
{
    public class VideoCreateDto
    {
        public int IdRecurso { get; set; }

        public int IdCapitulo { get; set; }

        [StringLength(300)]
        [Unicode(false)]
        public string? Descripcion { get; set; }

        public int? NumeroOrden { get; set; }

        [StringLength(50)]
        [Unicode(false)]
        public string? Tipo { get; set; } = null; //Aun no le vi utilidad a la propiedad
    }
}
