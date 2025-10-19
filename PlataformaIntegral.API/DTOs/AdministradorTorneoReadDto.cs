using PlataformaIntegral.API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlataformaIntegral.API.DTOs
{
    public class AdministradorTorneoReadDto
    {
        public int IdAdministrador { get; set; }

        public int IdTorneo { get; set; }

        public int? IdTipoRol { get; set; }

        public string NombreAdministrador { get; set; } = null!; //Nombre completo del administrador

        public string? TipoRol { get; set; } //Solo devolvera el nombre del tipo de rol

        public string? NombreTorneo { get; set; } = null!; //Nombre del torneo
    }
}
