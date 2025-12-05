namespace PlataformaIntegral.API.Services
{
    using global::PlataformaIntegral.API.DTOs;
    using global::PlataformaIntegral.API.Models;
    using Microsoft.EntityFrameworkCore;

    namespace PlataformaIntegral.API.Services
    {
        public class ComprasService : ICompraService
        {
            private readonly PlataformaIntegralContext _context;

            public ComprasService(PlataformaIntegralContext context)
            {
                _context = context;
            }

            public async Task<CompraResponseDto> ComprarCursoAsync(CompraCursoDto dto)
            {
                // Crear recibo simulado
                var recibo = new Recibo
                {
                    IdUsuario = dto.IdUsuario,
                    FechaEmision = DateOnly.FromDateTime(DateTime.Now),
                    NumeroRecibo = $"REC-{Guid.NewGuid().ToString().Substring(0, 8)}",
                    MontoTotal = dto.Monto,
                    Detalle = $"Compra del curso {dto.IdProducto}",
                    IdEstadoPago = 1 // Pagado
                };

                _context.Recibos.Add(recibo);
                await _context.SaveChangesAsync();

                // Crear pago simulado
                var pago = new Pago
                {
                    IdUsuario = dto.IdUsuario,
                    IdProducto = dto.IdProducto,
                    IdMetodoPago = dto.IdMetodoPago,
                    IdTipoMoneda = dto.IdTipoMoneda,
                    IdEstadoPago = 1, // Pagado
                    FechaPago = DateOnly.FromDateTime(DateTime.Now),
                    ReferenciaTransaccion = $"SIM-{Guid.NewGuid().ToString().Substring(0, 10)}",
                    Monto = dto.Monto,
                    IdRecibo = recibo.IdRecibo
                };

                _context.Pagos.Add(pago);
                await _context.SaveChangesAsync();

                return new CompraResponseDto
                {
                    IdRecibo = recibo.IdRecibo,
                    NumeroRecibo = recibo.NumeroRecibo ?? string.Empty,
                    FechaEmision = DateTime.Now,
                    MontoTotal = dto.Monto,
                    EstadoPago = "Pagado",
                    Mensaje = "Compra realizada con éxito (simulada)"
                };
            }
        }
    }

}
