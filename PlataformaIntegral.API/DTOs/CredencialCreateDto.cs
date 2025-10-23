using Microsoft.EntityFrameworkCore;
using PlataformaIntegral.API.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlataformaIntegral.API.DTOs
{
    public class CredencialCreateDto
    {

        [Required, StringLength(120), EmailAddress]
        public string Email { get; set; } = null!;

        [Required, StringLength(200, MinimumLength = 6)]
        public string? Contrasena { get; set; }

        [StringLength(50)]
        public string? Proveedor { get; set; }
    }
}
