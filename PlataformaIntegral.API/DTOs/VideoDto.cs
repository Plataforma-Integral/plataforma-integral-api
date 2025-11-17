namespace PlataformaIntegral.API.DTOs
{
    public class VideoDto
    {
        public string Nombre { get; set; } = string.Empty;      // Título del video
        public string? Descripcion { get; set; }               // Descripción opcional
        public int Duracion { get; set; }                      // Duración en segundos
        public IFormFile Archivo { get; set; } = default!;     // Archivo del video a subir
    }
}
