using Microsoft.EntityFrameworkCore;
using PlataformaIntegral.API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlataformaIntegral.API.DTOs
{
    public class CredencialReadDto
    {
        public int IdCredencial { get; set; }

        public int IdUsuario { get; set; }

        [StringLength(120)]
        [Unicode(false)]
        public string Email { get; set; } = null!;

        [StringLength(200)]
        [Unicode(false)]
        public string? Contrasena { get; set; }

        [StringLength(50)]
        [Unicode(false)]
        public string? Proveedor { get; set; }

        public DateTime? FechaCreacion { get; set; }

        public string? NombreUsuario { get; set; } = null!;
    }
}
