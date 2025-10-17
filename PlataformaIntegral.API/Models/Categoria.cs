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
    [InverseProperty("InverseIdSuperCategoriaNavigation")]
    public virtual Categoria? IdSuperCategoriaNavigation { get; set; }

    [InverseProperty("IdSuperCategoriaNavigation")]
    public virtual ICollection<Categoria> InverseIdSuperCategoriaNavigation { get; set; } = new List<Categoria>();

    [ForeignKey("IdCategoria")]
    [InverseProperty("IdCategoria")]
    public virtual ICollection<Curso> IdCursos { get; set; } = new List<Curso>();

    [ForeignKey("IdCategoria")]
    [InverseProperty("IdCategoria")]
    public virtual ICollection<Estudiante> IdEstudiantes { get; set; } = new List<Estudiante>();

    [ForeignKey("IdCategoria")]
    [InverseProperty("IdCategoria")]
    public virtual ICollection<Profesor> IdProfesors { get; set; } = new List<Profesor>();

    [ForeignKey("IdCategoria")]
    [InverseProperty("IdCategoria")]
    public virtual ICollection<Torneo> IdTorneos { get; set; } = new List<Torneo>();
}
