namespace PlataformaIntegral.API.DTOs
{
    public class CursoCardDto
    {
        public int Id { get; set; }

        public string Titulo { get; set; } = string.Empty;

        public string? PortadaUrl { get; set; }

        public decimal Precio { get; set; }

        public bool IsFree => Precio <= 0m;

        public List<string> Categorias { get; set; } = new();

        public List<ProfesorSimpleDto> Profesores { get; set; } = new();

        public double? Calificacion { get; set; }
        public int? CantidadEstudiantes { get; set; }

        public string? Modalidad { get; set; }

        public DateTime? FechaPublicacion { get; set; }
    }
}
