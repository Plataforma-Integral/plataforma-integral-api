using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PlataformaIntegral.API.Models;

[PrimaryKey("IdUsuario", "IdMedalla")]
[Table("estudiante_medalla")]
public partial class EstudianteMedalla
{
    [Key]
    [Column("id_usuario")]
    public int IdUsuario { get; set; }

    [Key]
    [Column("id_medalla")]
    public int IdMedalla { get; set; }

    [Column("fecha_otorgada")]
    public DateOnly? FechaOtorgada { get; set; }

    [ForeignKey("IdMedalla")]
    public virtual Medalla Medalla { get; set; } = null!;

    [ForeignKey("IdUsuario")]
    public virtual Estudiante Estudiante { get; set; } = null!;
}
