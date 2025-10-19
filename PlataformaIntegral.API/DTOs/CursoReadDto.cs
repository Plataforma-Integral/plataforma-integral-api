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

        //public CursoPregrabado? CursoPregrabado { get; set; }

        //public CursoSincronico? CursoSincronico { get; set; }

        public Producto Producto { get; set; } = null!;

        public ICollection<Categoria> Categorias { get; set; } = new List<Categoria>();
    }
}
