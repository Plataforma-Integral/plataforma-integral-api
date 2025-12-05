namespace PlataformaIntegral.API.DTOs
{
    public class CompraResponseDto
    {
        public int IdRecibo { get; set; }
        public string NumeroRecibo { get; set; } = string.Empty;
        public DateTime FechaEmision { get; set; }
        public decimal MontoTotal { get; set; }
        public string EstadoPago { get; set; } = "Pagado";
        public string Mensaje { get; set; } = "Compra realizada con éxito (simulada)";
    }
}
