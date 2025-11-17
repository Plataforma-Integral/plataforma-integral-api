namespace PlataformaIntegral.API.DTOs
{
    public class CapituloDto
    {
        public int Id { get; set; }
        public string? Titulo { get; set; } = string.Empty;
        public int? NumeroOrden { get; set; }
        public List<RecursoVideoDto>? Videos { get; set; } = new();
        public List<RecursoCuestionarioDto>? Cuestionarios { get; set; } = new();
    }
}
