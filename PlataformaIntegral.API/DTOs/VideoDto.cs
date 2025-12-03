namespace PlataformaIntegral.API.DTOs
{
    public class VideoDto
    {
        public string Nombre { get; set; } = string.Empty;      // Título del video
        public string? Descripcion { get; set; }               // Descripción opcional
        public IFormFile Archivo { get; set; } = default!;     // Archivo del video a subir
        public IFormFile? Miniatura { get; set; } = default!;
    }
}
