using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PlataformaIntegral.API.Models;

[Table("examen")]
public partial class Examen
{
    [Key]
    [Column("id_recurso")]
    public int IdRecurso { get; set; }

    [Column("id_curso_pregrabado")]
    public int? IdCursoPregrabado { get; set; }

    [Column("puntuacion_minima")]
    public int? PuntuacionMinima { get; set; }

    [ForeignKey("IdCursoPregrabado")]
    public virtual CursoPregrabado? CursoPregrabado { get; set; }

    [ForeignKey("IdRecurso")]
    public virtual Recurso Recurso { get; set; } = null!;
}
