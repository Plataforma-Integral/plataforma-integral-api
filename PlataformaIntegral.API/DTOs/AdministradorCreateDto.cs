using Microsoft.EntityFrameworkCore;
using PlataformaIntegral.API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlataformaIntegral.API.DTOs
{
    public class AdministradorCreateDto
    {
        [Required]
        public int IdUsuario { get; set; }

        [StringLength(80)]
        [Unicode(false)]
        public string? Rol { get; set; }
    }
}
