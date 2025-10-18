using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PlataformaIntegral.API.Models;

[Table("curso")]
public partial class Curso
{
    [Key]
    [Column("id_producto")]
    public int IdProducto { get; set; }

    public virtual ICollection<Certificado> Certificados { get; set; } = new List<Certificado>();

    public virtual CursoPregrabado? CursoPregrabado { get; set; }

    public virtual CursoSincronico? CursoSincronico { get; set; }

    [ForeignKey("IdProducto")]
    public virtual Producto Producto { get; set; } = null!;

    public virtual ICollection<ProfesorCurso> ProfesorCursos { get; set; } = new List<ProfesorCurso>();

    public virtual ICollection<ReseñaCurso> ReseñaCursos { get; set; } = new List<ReseñaCurso>();

    [ForeignKey("IdCurso")]
    public virtual ICollection<Categoria> Categorias { get; set; } = new List<Categoria>();

    [ForeignKey("IdCurso")]
    public virtual ICollection<SuscripcionTipo> SuscripcionesTipo { get; set; } = new List<SuscripcionTipo>();
}
