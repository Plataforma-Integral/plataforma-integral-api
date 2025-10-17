using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PlataformaIntegral.API.Models;

[Table("recibo")]
public partial class Recibo
{
    [Key]
    [Column("id_recibo")]
    public int IdRecibo { get; set; }

    [Column("id_usuario")]
    public int IdUsuario { get; set; }

    [Column("fecha_emision")]
    public DateOnly? FechaEmision { get; set; }

    [Column("numero_recibo")]
    [StringLength(50)]
    [Unicode(false)]
    public string? NumeroRecibo { get; set; }

    [Column("monto_total", TypeName = "decimal(10, 2)")]
    public decimal? MontoTotal { get; set; }

    [Column("detalle")]
    [StringLength(400)]
    [Unicode(false)]
    public string? Detalle { get; set; }

    [Column("id_estado_pago")]
    public int? IdEstadoPago { get; set; }

    [ForeignKey("IdEstadoPago")]
    [InverseProperty("Recibos")]
    public virtual EstadoPago? IdEstadoPagoNavigation { get; set; }

    [ForeignKey("IdUsuario")]
    [InverseProperty("Recibos")]
    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;

    [InverseProperty("IdReciboNavigation")]
    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();
}
