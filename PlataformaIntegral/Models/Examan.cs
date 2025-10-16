using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PlataformaIntegral.Models;

[Table("examen")]
public partial class Examan
{
    [Key]
    [Column("id_recurso")]
    public int IdRecurso { get; set; }

    [Column("id_curso_pregrabado")]
    public int? IdCursoPregrabado { get; set; }

    [Column("puntuacion_minima")]
    public int? PuntuacionMinima { get; set; }

    [ForeignKey("IdCursoPregrabado")]
    [InverseProperty("Examen")]
    public virtual CursoPregrabado? IdCursoPregrabadoNavigation { get; set; }

    [ForeignKey("IdRecurso")]
    [InverseProperty("Examan")]
    public virtual Recurso IdRecursoNavigation { get; set; } = null!;
}
