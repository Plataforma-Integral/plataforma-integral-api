using Microsoft.EntityFrameworkCore;
using PlataformaIntegral.API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlataformaIntegral.API.DTOs
{
    public class MedallaCreateDto
    {
        public int? IdTorneo { get; set; }

        public int? IdNivelMedalla { get; set; }

        [StringLength(100)]
        [Unicode(false)]
        public string? Nombre { get; set; }

        [StringLength(300)]
        [Unicode(false)]
        public string? Descripcion { get; set; }

        [StringLength(300)]
        [Unicode(false)]
        public string? UrlImagen { get; set; }
    }
}
