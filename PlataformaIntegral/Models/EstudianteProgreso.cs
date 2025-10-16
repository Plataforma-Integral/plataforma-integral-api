using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PlataformaIntegral.Models;

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
    [InverseProperty("EstudianteProgresos")]
    public virtual Estudiante IdEstudianteNavigation { get; set; } = null!;

    [ForeignKey("IdRecurso")]
    [InverseProperty("EstudianteProgresos")]
    public virtual Recurso IdRecursoNavigation { get; set; } = null!;
}
