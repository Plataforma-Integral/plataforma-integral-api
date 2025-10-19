using Microsoft.EntityFrameworkCore;
using PlataformaIntegral.API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlataformaIntegral.API.DTOs
{
    public class ClasePresencialCreateDto
    {
        [Required]
        public int IdClase { get; set; }

        [Required]
        [StringLength(200)]
        [Unicode(false)]
        public string? Direccion { get; set; }
    }
}
