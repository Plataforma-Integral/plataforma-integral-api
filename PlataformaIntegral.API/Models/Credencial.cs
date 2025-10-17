using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PlataformaIntegral.API.Models;

[Table("credencial")]
[Index("Email", Name = "UQ_credencial_email", IsUnique = true)]
[Index("Email", Name = "idx_credencial_email")]
public partial class Credencial
{
    [Key]
    [Column("id_credencial")]
    public int IdCredencial { get; set; }

    [Column("id_usuario")]
    public int IdUsuario { get; set; }

    [Column("email")]
    [StringLength(120)]
    [Unicode(false)]
    public string Email { get; set; } = null!;

    [Column("contrasena")]
    [StringLength(200)]
    [Unicode(false)]
    public string? Contrasena { get; set; }

    [Column("proveedor")]
    [StringLength(50)]
    [Unicode(false)]
    public string? Proveedor { get; set; }

    [Column("fecha_creacion", TypeName = "datetime")]
    public DateTime? FechaCreacion { get; set; }

    [ForeignKey("IdUsuario")]
    [InverseProperty("Credencials")]
    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
