using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PlataformaIntegral.API.Models;

[PrimaryKey("IdEstudiante", "IdRecurso")]
[Table("estudiante_progreso")]
public partial class EstudianteProgreso
{
    [Key]
    [Column("id_estudiante")]
    public int IdEstudiante { get; set; }

    [Key]
    [Column("id_recurso")]
    public int IdRecurso { get; set; }

    [Column("estado")]
    [StringLength(50)]
    [Unicode(false)]
    public string? Estado { get; set; }

    [ForeignKey("IdEstudiante")]
    public virtual Estudiante Estudiante { get; set; } = null!;

    [ForeignKey("IdRecurso")]
    public virtual Recurso Recurso { get; set; } = null!;
}
