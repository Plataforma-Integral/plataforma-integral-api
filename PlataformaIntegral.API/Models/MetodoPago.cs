using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PlataformaIntegral.API.Models;

[Table("metodo_pago")]
[Index("Nombre", Name = "UQ__metodo_p__72AFBCC61E1DDAB0", IsUnique = true)]
public partial class MetodoPago
{
    [Key]
    [Column("id_metodo_pago")]
    public int IdMetodoPago { get; set; }

    [Column("nombre")]
    [StringLength(50)]
    [Unicode(false)]
    public string Nombre { get; set; } = null!;

    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();
}
