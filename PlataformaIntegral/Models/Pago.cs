using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PlataformaIntegral.Models;

[Table("pago")]
public partial class Pago
{
    [Key]
    [Column("id_pago")]
    public int IdPago { get; set; }

    [Column("id_recibo")]
    public int? IdRecibo { get; set; }

    [Column("id_usuario")]
    public int IdUsuario { get; set; }

    [Column("id_tipo_moneda")]
    public int IdTipoMoneda { get; set; }

    [Column("id_producto")]
    public int IdProducto { get; set; }

    [Column("id_metodo_pago")]
    public int IdMetodoPago { get; set; }

    [Column("id_estado_pago")]
    public int IdEstadoPago { get; set; }

    [Column("fecha_pago")]
    public DateOnly? FechaPago { get; set; }

    [Column("referencia_transaccion")]
    [StringLength(100)]
    [Unicode(false)]
    public string? ReferenciaTransaccion { get; set; }

    [Column("monto", TypeName = "decimal(10, 2)")]
    public decimal? Monto { get; set; }

    [ForeignKey("IdEstadoPago")]
    [InverseProperty("Pagos")]
    public virtual EstadoPago IdEstadoPagoNavigation { get; set; } = null!;

    [ForeignKey("IdMetodoPago")]
    [InverseProperty("Pagos")]
    public virtual MetodoPago IdMetodoPagoNavigation { get; set; } = null!;

    [ForeignKey("IdProducto")]
    [InverseProperty("Pagos")]
    public virtual Producto IdProductoNavigation { get; set; } = null!;

    [ForeignKey("IdRecibo")]
    [InverseProperty("Pagos")]
    public virtual Recibo? IdReciboNavigation { get; set; }

    [ForeignKey("IdTipoMoneda")]
    [InverseProperty("Pagos")]
    public virtual TipoMonedum IdTipoMonedaNavigation { get; set; } = null!;

    [ForeignKey("IdUsuario")]
    [InverseProperty("Pagos")]
    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
