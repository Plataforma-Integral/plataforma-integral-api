using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PlataformaIntegral.API.Models;

[Table("clase_presencial")]
public partial class ClasePresencial
{
    [Key]
    [Column("id_clase")]
    public int IdClase { get; set; }

    [Column("direccion")]
    [StringLength(200)]
    [Unicode(false)]
    public string? Direccion { get; set; }

    [ForeignKey("IdClase")]
    public virtual Clase Clase { get; set; } = null!;
}
