using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PlataformaIntegral.API.Models;

[Table("tipo_rol")]
[Index("Nombre", Name = "UQ__tipo_rol__72AFBCC68FD40E61", IsUnique = true)]
public partial class TipoRol
{
    [Key]
    [Column("id_tipo_rol")]
    public int IdTipoRol { get; set; }

    [Column("nombre")]
    [StringLength(50)]
    [Unicode(false)]
    public string Nombre { get; set; } = null!;

    [InverseProperty("IdTipoRolNavigation")]
    public virtual ICollection<AdministradorTorneo> AdministradorTorneos { get; set; } = new List<AdministradorTorneo>();

    [InverseProperty("IdTipoRolNavigation")]
    public virtual ICollection<ProfesorCurso> ProfesorCursos { get; set; } = new List<ProfesorCurso>();
}
