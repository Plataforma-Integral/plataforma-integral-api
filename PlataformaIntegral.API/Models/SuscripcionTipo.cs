using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PlataformaIntegral.API.Models;

[Table("suscripcion_tipo")]
public partial class SuscripcionTipo
{
    [Key]
    [Column("id_producto")]
    public int IdProducto { get; set; }

    [Column("duracion_meses")]
    public int DuracionMeses { get; set; }

    [ForeignKey("IdProducto")]
    public virtual Producto Producto { get; set; } = null!;

    public virtual ICollection<Suscripcion> Suscripciones { get; set; } = new List<Suscripcion>();

    [ForeignKey("IdSuscripcionTipo")]
    public virtual ICollection<Curso> Cursos { get; set; } = new List<Curso>();
}
