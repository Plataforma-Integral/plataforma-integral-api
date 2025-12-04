namespace PlataformaIntegral.API.DTOs
{
    public class CatalogoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Codigo { get; set; } // opcional (ej: ISO país, moneda)
    }

}
