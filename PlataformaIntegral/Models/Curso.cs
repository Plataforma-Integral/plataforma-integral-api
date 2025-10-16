using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PlataformaIntegral.Models;

[Table("curso")]
public partial class Curso
{
    [Key]
    [Column("id_producto")]
    public int IdProducto { get; set; }

    [InverseProperty("IdCursoNavigation")]
    public virtual ICollection<Certificado> Certificados { get; set; } = new List<Certificado>();

    [InverseProperty("IdCursoNavigation")]
    public virtual CursoPregrabado? CursoPregrabado { get; set; }

    [InverseProperty("IdCursoNavigation")]
    public virtual CursoSincronico? CursoSincronico { get; set; }

    [ForeignKey("IdProducto")]
    [InverseProperty("Curso")]
    public virtual Producto IdProductoNavigation { get; set; } = null!;

    [InverseProperty("IdCursoNavigation")]
    public virtual ICollection<ProfesorCurso> ProfesorCursos { get; set; } = new List<ProfesorCurso>();

    [InverseProperty("IdCursoNavigation")]
    public virtual ICollection<ReseñaCurso> ReseñaCursos { get; set; } = new List<ReseñaCurso>();

    [ForeignKey("IdCurso")]
    [InverseProperty("IdCursos")]
    public virtual ICollection<Categorium> IdCategoria { get; set; } = new List<Categorium>();

    [ForeignKey("IdCurso")]
    [InverseProperty("IdCursos")]
    public virtual ICollection<SuscripcionTipo> IdSuscripcionTipos { get; set; } = new List<SuscripcionTipo>();
}
