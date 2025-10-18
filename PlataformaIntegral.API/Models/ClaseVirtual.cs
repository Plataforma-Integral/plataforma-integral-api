using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PlataformaIntegral.API.Models;

[Table("clase_virtual")]
public partial class ClaseVirtual
{
    [Key]
    [Column("id_clase")]
    public int IdClase { get; set; }

    [Column("url")]
    [StringLength(300)]
    [Unicode(false)]
    public string? Url { get; set; }

    [ForeignKey("IdClase")]
    public virtual Clase Clase { get; set; } = null!;
}
