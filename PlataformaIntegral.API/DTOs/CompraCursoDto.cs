public class CompraCursoDto
{
    public int IdUsuario { get; set; }
    public int IdProducto { get; set; } // curso
    public int IdMetodoPago { get; set; }
    public int IdTipoMoneda { get; set; }
    public decimal Monto { get; set; }
}