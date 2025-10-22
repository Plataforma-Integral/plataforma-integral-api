using Microsoft.EntityFrameworkCore;
using PlataformaIntegral.API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlataformaIntegral.API.DTOs
{
    public class PaisCreateDto
    {
        [StringLength(80)]
        [Unicode(false)]
        public string Nombre { get; set; } = null!;

        [StringLength(2)]
        [Unicode(false)]
        public string? CodigoIso { get; set; }
    }
}
