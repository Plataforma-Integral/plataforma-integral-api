namespace PlataformaIntegral.API.DTOs
{
    public class CursoPregrabadoDto
    {
        public string Titulo { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int PrecioPuntos { get; set; }

        // Opcional: portada (imagen)
        public IFormFile? Portada { get; set; }
    }
}
