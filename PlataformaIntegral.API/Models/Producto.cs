using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PlataformaIntegral.API.Models;

[Table("producto")]
[Index("Nombre", Name = "idx_producto_nombre")]
public partial class Producto
{
    [Key]
    [Column("id_producto")]
    public int IdProducto { get; set; }

    [Column("nombre")]
    [StringLength(120)]
    [Unicode(false)]
    public string Nombre { get; set; } = null!;

    [Column("descripcion")]
    [StringLength(400)]
    [Unicode(false)]
    public string? Descripcion { get; set; }

    [Column("precio", TypeName = "decimal(10, 2)")]
    public decimal Precio { get; set; }

    [Column("fecha_creacion", TypeName = "datetime")]
    public DateTime? FechaCreacion { get; set; }

    [InverseProperty("IdProductoNavigation")]
    public virtual Curso? Curso { get; set; }

    [InverseProperty("IdProductoNavigation")]
    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();

    [InverseProperty("IdProductoNavigation")]
    public virtual SuscripcionTipo? SuscripcionTipo { get; set; }
}
