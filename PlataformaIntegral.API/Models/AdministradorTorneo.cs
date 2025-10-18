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
    public virtual Administrador Administrador { get; set; } = null!;

    [ForeignKey("IdTipoRol")]
    public virtual TipoRol? TipoRol { get; set; }

    [ForeignKey("IdTorneo")]
    public virtual Torneo Torneo { get; set; } = null!;
}
