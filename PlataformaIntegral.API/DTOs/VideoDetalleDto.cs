namespace PlataformaIntegral.API.DTOs
{
    public class VideoDetalleDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public TimeSpan? Duracion { get; set; }
        public string? PresignedUrl { get; set; }
        public string? MimeType { get; set; }
        public long? TamañoBytes { get; set; }
    }
}
