using Microsoft.EntityFrameworkCore;
using PlataformaIntegral.API.Enums;
using PlataformaIntegral.API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlataformaIntegral.API.DTOs
{
    public class RecursoCreateDto
    {
        [StringLength(120)]
        [Unicode(false)]
        public string? Nombre { get; set; }

        [StringLength(300)]
        [Unicode(false)]
        public string? Url { get; set; }

        public TipoRecursoEnum TipoRecurso { get; set; }

        public CertificadoCreateDto? Certificado { get; set; }

        public CuestionarioCreateDto? Cuestionario { get; set; }

        public DocumentoCreateDto? Documento { get; set; }

        public ExamenCreateDto? Examen { get; set; }

        //public VideoCreateDto? Video { get; set; }
    }
}
