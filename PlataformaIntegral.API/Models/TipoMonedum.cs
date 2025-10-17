using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PlataformaIntegral.API.Models;

[Table("tipo_moneda")]
[Index("Codigo", Name = "UQ__tipo_mon__40F9A20675801CCA", IsUnique = true)]
public partial class TipoMonedum
{
    [Key]
    [Column("id_tipo_moneda")]
    public int IdTipoMoneda { get; set; }

    [Column("codigo")]
    [StringLength(3)]
    [Unicode(false)]
    public string? Codigo { get; set; }

    [Column("nombre")]
    [StringLength(50)]
    [Unicode(false)]
    public string Nombre { get; set; } = null!;

    [InverseProperty("IdTipoMonedaNavigation")]
    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();
}
