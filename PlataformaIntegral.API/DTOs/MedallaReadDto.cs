using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace PlataformaIntegral.API.DTOs
{
    public class MedallaReadDto
    {
        public int IdMedalla { get; set; }

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

        public virtual string? NombreNivelMedalla { get; set; }

        public virtual string? NombreTorneo { get; set; }
    }
}
