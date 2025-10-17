using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PlataformaIntegral.API.Models;

[PrimaryKey("IdEstudiante", "IdProfesor")]
[Table("reseña_profesor")]
public partial class ReseñaProfesor
{
    [Key]
    [Column("id_estudiante")]
    public int IdEstudiante { get; set; }

    [Key]
    [Column("id_profesor")]
    public int IdProfesor { get; set; }

    [Column("opinion")]
    public bool Opinion { get; set; }

    [Column("comentario")]
    [StringLength(300)]
    [Unicode(false)]
    public string? Comentario { get; set; }

    [ForeignKey("IdEstudiante")]
    [InverseProperty("ReseñaProfesors")]
    public virtual Estudiante IdEstudianteNavigation { get; set; } = null!;

    [ForeignKey("IdProfesor")]
    [InverseProperty("ReseñaProfesors")]
    public virtual Profesor IdProfesorNavigation { get; set; } = null!;
}
