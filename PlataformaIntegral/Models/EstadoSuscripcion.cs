using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PlataformaIntegral.Models;

[Table("estado_suscripcion")]
[Index("Nombre", Name = "UQ__estado_s__72AFBCC6A44D15F0", IsUnique = true)]
public partial class EstadoSuscripcion
{
    [Key]
    [Column("id_estado_suscripcion")]
    public int IdEstadoSuscripcion { get; set; }

    [Column("nombre")]
    [StringLength(50)]
    [Unicode(false)]
    public string Nombre { get; set; } = null!;

    [InverseProperty("IdEstadoSuscripcionNavigation")]
    public virtual ICollection<Suscripcion> Suscripcions { get; set; } = new List<Suscripcion>();
}
