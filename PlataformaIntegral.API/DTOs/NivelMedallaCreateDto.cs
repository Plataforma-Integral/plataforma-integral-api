using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace PlataformaIntegral.API.DTOs
{
    public class NivelMedallaCreateDto
    {
        [StringLength(50)]
        [Unicode(false)]
        public string Nombre { get; set; } = null!;
    }
}
