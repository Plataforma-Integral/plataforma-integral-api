using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PlataformaIntegral.Models;

[Table("pais")]
public partial class Pai
{
    [Key]
    [Column("id_pais")]
    public int IdPais { get; set; }

    [Column("nombre")]
    [StringLength(80)]
    [Unicode(false)]
    public string Nombre { get; set; } = null!;

    [Column("codigo_iso")]
    [StringLength(2)]
    [Unicode(false)]
    public string? CodigoIso { get; set; }

    [InverseProperty("IdPaisNavigation")]
    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
