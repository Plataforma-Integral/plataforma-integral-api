using PlataformaIntegral.API.Enums;
using PlataformaIntegral.API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlataformaIntegral.API.DTOs
{
    public class CursoReadDto
    {
        public int IdProducto { get; set; }

        public bool EsPrivado { get; set; }

        public CertificadoReadDto Certificado { get; set; } = null!;

        public CursoPregrabadoReadDto? CursoPregrabado { get; set; } = null;

        public CursoSincronicoReadDto? CursoSincronico { get; set; } = null;

        public string? NombreProducto { get; set; } = null!;
    }
}
