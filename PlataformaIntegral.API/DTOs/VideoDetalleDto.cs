namespace PlataformaIntegral.API.DTOs
{
    public class VideoDetalleDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string? Descripcion { get; set; }

        // Duración como TimeSpan
        public TimeSpan Duracion { get; set; }

        // Propiedad calculada para mostrar en formato hh:mm:ss
        public string DuracionTexto => Duracion.ToString(@"hh\:mm\:ss");

        // URL firmada para streaming desde MinIO
        public string? VideoUrl { get; set; }

        // Miniatura para mostrar en la página
        public string? MiniaturaUrl { get; set; }

        // Metadatos técnicos
        public long PesoBytes { get; set; }
    }
}
