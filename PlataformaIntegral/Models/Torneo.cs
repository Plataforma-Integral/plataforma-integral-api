using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PlataformaIntegral.Models;

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

    [InverseProperty("IdTorneoNavigation")]
    public virtual ICollection<AdministradorTorneo> AdministradorTorneos { get; set; } = new List<AdministradorTorneo>();

    [InverseProperty("IdTorneoNavigation")]
    public virtual ICollection<Medalla> Medallas { get; set; } = new List<Medalla>();

    [ForeignKey("IdTorneo")]
    [InverseProperty("IdTorneos")]
    public virtual ICollection<Categorium> IdCategoria { get; set; } = new List<Categorium>();

    [ForeignKey("IdTorneo")]
    [InverseProperty("IdTorneos")]
    public virtual ICollection<Estudiante> IdEstudiantes { get; set; } = new List<Estudiante>();
}
