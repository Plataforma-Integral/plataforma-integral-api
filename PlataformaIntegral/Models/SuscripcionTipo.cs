using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PlataformaIntegral.Models;

[Table("suscripcion_tipo")]
public partial class SuscripcionTipo
{
    [Key]
    [Column("id_producto")]
    public int IdProducto { get; set; }

    [Column("duracion_meses")]
    public int DuracionMeses { get; set; }

    [ForeignKey("IdProducto")]
    [InverseProperty("SuscripcionTipo")]
    public virtual Producto IdProductoNavigation { get; set; } = null!;

    [InverseProperty("IdSuscripcionTipoNavigation")]
    public virtual ICollection<Suscripcion> Suscripcions { get; set; } = new List<Suscripcion>();

    [ForeignKey("IdSuscripcionTipo")]
    [InverseProperty("IdSuscripcionTipos")]
    public virtual ICollection<Curso> IdCursos { get; set; } = new List<Curso>();
}
