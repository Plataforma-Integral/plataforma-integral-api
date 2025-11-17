namespace PlataformaIntegral.API.DTOs
{
    public class RecursoVideoDto
    {
        public int Id { get; set; }
        public string? Titulo { get; set; } = string.Empty;
        public TimeSpan? Duracion { get; set; }
        public bool? Visto { get; set; } // según estudiante (progreso)
        public int? NumeroOrden { get; set; }
        public string? PresignedUrl { get; set; } // opcional: si pedir reproducción
    }
}
