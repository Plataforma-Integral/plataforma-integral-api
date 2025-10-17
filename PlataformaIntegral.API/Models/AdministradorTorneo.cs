using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using PlataformaIntegral.API.Models;

namespace PlataformaIntegral.API.Models;

[PrimaryKey("IdAdministrador", "IdTorneo")]
[Table("administrador_torneo")]
public partial class AdministradorTorneo
{
    [Key]
    [Column("id_administrador")]
    public int IdAdministrador { get; set; }

    [Key]
    [Column("id_torneo")]
    public int IdTorneo { get; set; }

    [Column("id_tipo_rol")]
    public int? IdTipoRol { get; set; }

    [ForeignKey("IdAdministrador")]
    [InverseProperty("AdministradorTorneos")]
    public virtual Administrador IdAdministradorNavigation { get; set; } = null!;

    [ForeignKey("IdTipoRol")]
    [InverseProperty("AdministradorTorneos")]
    public virtual TipoRol? IdTipoRolNavigation { get; set; }

    [ForeignKey("IdTorneo")]
    [InverseProperty("AdministradorTorneos")]
    public virtual Torneo IdTorneoNavigation { get; set; } = null!;
}
