using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PlataformaIntegral.API.Models;

[PrimaryKey("IdEstudiante", "IdCurso")]
[Table("reseña_curso")]
public partial class ReseñaCurso
{
    [Key]
    [Column("id_estudiante")]
    public int IdEstudiante { get; set; }

    [Key]
    [Column("id_curso")]
    public int IdCurso { get; set; }

    [Column("opinion")]
    public bool Opinion { get; set; }

    [Column("comentario")]
    [StringLength(300)]
    [Unicode(false)]
    public string? Comentario { get; set; }

    [ForeignKey("IdCurso")]
    public virtual Curso Curso { get; set; } = null!;

    [ForeignKey("IdEstudiante")]
    public virtual Estudiante Estudiante { get; set; } = null!;
}
