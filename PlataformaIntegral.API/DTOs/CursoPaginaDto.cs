namespace PlataformaIntegral.API.DTOs
{
    public class CursoPaginaDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string? PortadaUrl { get; set; }
        public decimal Precio { get; set; }
        public decimal? PrecioPuntos { get; set; }
        public string? Descripcion { get; set; }
        public List<string?>? Categorias { get; set; } = new();
        public List<CapituloDto>? Capitulos { get; set; } = new();
        public DateTime? FechaCreacion { get; set; }
        public double? Calificacion { get; set; } // 0..100 o null
        public bool? Reseña { get; set; }
        public List<ProfesorSimpleDto>? Profesores { get; set; }
    }
}
