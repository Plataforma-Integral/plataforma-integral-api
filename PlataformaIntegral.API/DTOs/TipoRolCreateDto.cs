using Microsoft.EntityFrameworkCore;
using PlataformaIntegral.API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlataformaIntegral.API.DTOs
{
    public class TipoRolCreateDto
    {
        [StringLength(50)]
        [Unicode(false)]
        public string Nombre { get; set; } = null!;
    }
}
