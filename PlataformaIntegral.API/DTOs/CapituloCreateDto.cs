using Microsoft.EntityFrameworkCore;
using PlataformaIntegral.API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlataformaIntegral.API.DTOs
{
    public class CapituloCreateDto
    {
        public int IdCursoPregrabado { get; set; }
        public string Nombre { get; set; } = string.Empty;
    }
}
