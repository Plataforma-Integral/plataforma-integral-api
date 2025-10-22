using PlataformaIntegral.API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlataformaIntegral.API.DTOs
{
    public class ProfesorCursoCreateDto
    {
        public int IdProfesor { get; set; }

        public int IdCurso { get; set; }

        public int? IdTipoRol { get; set; }
    }
}
