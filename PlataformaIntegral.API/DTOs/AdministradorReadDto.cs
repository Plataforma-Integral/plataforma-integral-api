using Microsoft.EntityFrameworkCore;
using PlataformaIntegral.API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlataformaIntegral.API.DTOs
{
    public class AdministradorReadDto
    {
        [StringLength(80)]
        [Unicode(false)]
        public string? Rol { get; set; }

        public string? NombreUsuario { get; set; } = null!;
    }
}
