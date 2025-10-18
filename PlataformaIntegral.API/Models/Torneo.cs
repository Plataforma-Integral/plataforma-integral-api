using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PlataformaIntegral.API.Models;

[Table("torneo")]
public partial class Torneo
{
    [Key]
    [Column("id_torneo")]
    public int IdTorneo { get; set; }

    [Column("nombre")]
    [StringLength(120)]
    [Unicode(false)]
    public string? Nombre { get; set; }

    [Column("descripcion")]
    [StringLength(300)]
    [Unicode(false)]
    public string? Descripcion { get; set; }

    [Column("modalidad")]
    [StringLength(80)]
    [Unicode(false)]
    public string? Modalidad { get; set; }

    public virtual ICollection<AdministradorTorneo> AdministradorTorneos { get; set; } = new List<AdministradorTorneo>();

    public virtual ICollection<Medalla> Medallas { get; set; } = new List<Medalla>();

    [ForeignKey("IdTorneo")]
    public virtual ICollection<Categoria> Categorias { get; set; } = new List<Categoria>();

    [ForeignKey("IdTorneo")]
    public virtual ICollection<Estudiante> Estudiantes { get; set; } = new List<Estudiante>();
}
