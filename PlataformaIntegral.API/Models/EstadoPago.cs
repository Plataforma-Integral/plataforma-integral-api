using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PlataformaIntegral.API.Models;

[Table("estado_pago")]
[Index("Nombre", Name = "UQ__estado_p__72AFBCC61390F3C7", IsUnique = true)]
public partial class EstadoPago
{
    [Key]
    [Column("id_estado_pago")]
    public int IdEstadoPago { get; set; }

    [Column("nombre")]
    [StringLength(50)]
    [Unicode(false)]
    public string Nombre { get; set; } = null!;

    [InverseProperty("IdEstadoPagoNavigation")]
    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();

    [InverseProperty("IdEstadoPagoNavigation")]
    public virtual ICollection<Recibo> Recibos { get; set; } = new List<Recibo>();
}
