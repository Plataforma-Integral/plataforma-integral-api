namespace PlataformaIntegral.API.DTOs
{
    public class RecursoCuestionarioDto
    {
        public int Id { get; set; }
        public string? Titulo { get; set; } = string.Empty;
        public bool? Resuelto { get; set; } // según estudiante
        public int? NumeroOrden { get; set; }
    }
}
