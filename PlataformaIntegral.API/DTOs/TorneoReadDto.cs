using Microsoft.EntityFrameworkCore;
using PlataformaIntegral.API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlataformaIntegral.API.DTOs
{
    public class TorneoReadDto
    {
        public int IdTorneo { get; set; }

        [StringLength(120)]
        [Unicode(false)]
        public string? Nombre { get; set; }

        [StringLength(300)]
        [Unicode(false)]
        public string? Descripcion { get; set; }

        [StringLength(80)]
        [Unicode(false)]
        public string? Modalidad { get; set; }
    }
}
