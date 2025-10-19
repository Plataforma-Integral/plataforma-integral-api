using PlataformaIntegral.API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlataformaIntegral.API.DTOs
{
    public class AdministradorTorneoCreateDto
    {
        [Required]
        public int IdAdministrador { get; set; }
        [Required]
        public int IdTorneo { get; set; }

        public int? IdTipoRol { get; set; }
    }
}
