namespace PlataformaIntegral.API.DTOs
{
    public class ConfigPrivacidadReadDto
    {
        public int IdUsuario { get; set; }
        public bool? MostrarEmail { get; set; }

        public bool? MostrarTelefono { get; set; }

        public bool? MostrarNombre { get; set; }

        public bool? MostrarFechaNacimiento { get; set; }

        public bool? MostrarMedallas { get; set; }
    }
}
