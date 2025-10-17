using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using PlataformaIntegral.API.Models; // Agrega esta directiva using para que el tipo Usuario sea reconocido

namespace PlataformaIntegral.API.Models;

[Table("administrador")]
public partial class Administrador
{
    [Key]
    [Column("id_usuario")]
    public int IdUsuario { get; set; }

    [Column("rol")]
    [StringLength(80)]
    [Unicode(false)]
    public string? Rol { get; set; }

    [InverseProperty("IdAdministradorNavigation")]
    public virtual ICollection<AdministradorTorneo> AdministradorTorneos { get; set; } = new List<AdministradorTorneo>();

    [ForeignKey("IdUsuario")]
    [InverseProperty("Administrador")]
    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
