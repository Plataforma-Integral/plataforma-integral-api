using Microsoft.EntityFrameworkCore;
using PlataformaIntegral.API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlataformaIntegral.API.DTOs
{
    public class ClaseVirtualReadDto
    {
        public int IdClase { get; set; }

        [StringLength(300)]
        [Unicode(false)]
        public string? Url { get; set; }

        public string? NombreClase { get; set; } = null!;
    }
}
