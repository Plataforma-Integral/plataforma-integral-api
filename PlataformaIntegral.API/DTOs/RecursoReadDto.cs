using Microsoft.EntityFrameworkCore;
using PlataformaIntegral.API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlataformaIntegral.API.DTOs
{
    public class RecursoReadDto
    {
        public int IdRecurso { get; set; }

        [StringLength(120)]
        [Unicode(false)]
        public string? Nombre { get; set; }

        [StringLength(300)]
        [Unicode(false)]
        public string? Url { get; set; }

        public virtual CertificadoReadDto? Certificado { get; set; }

        public virtual CuestionarioReadDto? Cuestionario { get; set; }

        public virtual DocumentoReadDto? Documento { get; set; }

        public virtual ExamenReadDto? Examen { get; set; }

        //public virtual VideoReadDto? Video { get; set; }
    }
}
