using PlataformaIntegral.API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlataformaIntegral.API.DTOs
{
    public class EstudianteMedallaCreateDto
    {
        public int IdUsuario { get; set; }

        public int IdMedalla { get; set; }
    }
}
