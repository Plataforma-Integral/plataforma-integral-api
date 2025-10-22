namespace PlataformaIntegral.API.DTOs
{
    public class EstudianteMedallaReadDto
    {
        public int IdUsuario { get; set; }

        public int IdMedalla { get; set; }

        public DateOnly? FechaOtorgada { get; set; }

        public string? NombreMedalla { get; set; } = null!;

        public string? NombreUsuario { get; set; } = null!;
    }
}
