using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PlataformaIntegral.API.Models;

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
    public virtual EstadoPago EstadoPago { get; set; } = null!;

    [ForeignKey("IdMetodoPago")]
    public virtual MetodoPago MetodoPago { get; set; } = null!;

    [ForeignKey("IdProducto")]
    public virtual Producto Producto { get; set; } = null!;

    [ForeignKey("IdRecibo")]
    public virtual Recibo? Recibo { get; set; }

    [ForeignKey("IdTipoMoneda")]
    public virtual TipoMoneda TipoMoneda { get; set; } = null!;

    [ForeignKey("IdUsuario")]
    public virtual Usuario Usuario { get; set; } = null!;
}
