using Microsoft.EntityFrameworkCore;
using PlataformaIntegral.API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlataformaIntegral.API.DTOs
{
    public class ClasePresencialReadDto
    {
        public int IdClase { get; set; }

        [StringLength(200)]
        [Unicode(false)]
        public string? Direccion { get; set; }

        public string? NombreClase { get; set; } = null!;
    }
}
