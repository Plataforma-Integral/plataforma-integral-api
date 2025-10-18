using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PlataformaIntegral.API.Models;

[Table("categoria")]
public partial class Categoria
{
    [Key]
    [Column("id_categoria")]
    public int IdCategoria { get; set; }

    [Column("nombre")]
    [StringLength(120)]
    [Unicode(false)]
    public string? Nombre { get; set; }

    [Column("descripcion")]
    [StringLength(300)]
    [Unicode(false)]
    public string? Descripcion { get; set; }

    [Column("id_super_categoria")]
    public int? IdSuperCategoria { get; set; }

    [ForeignKey("IdSuperCategoria")]
    [InverseProperty("SubCategorias")]
    public virtual Categoria? SuperCategoria { get; set; }

    [InverseProperty("SuperCategoria")]
    public virtual ICollection<Categoria> SubCategorias { get; set; } = new List<Categoria>();

    public virtual ICollection<Curso> Cursos { get; set; } = new List<Curso>();

    public virtual ICollection<Estudiante> Estudiantes { get; set; } = new List<Estudiante>();

    public virtual ICollection<Profesor> Profesores { get; set; } = new List<Profesor>();

    public virtual ICollection<Torneo> Torneos { get; set; } = new List<Torneo>();
}
