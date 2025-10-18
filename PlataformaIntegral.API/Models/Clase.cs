using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PlataformaIntegral.API.Models;

[Table("clase")]
public partial class Clase
{
    [Key]
    [Column("id_clase")]
    public int IdClase { get; set; }

    [Column("id_profesor")]
    public int IdProfesor { get; set; }

    [Column("id_curso_sincronico")]
    public int IdCursoSincronico { get; set; }

    [Column("nombre")]
    [StringLength(120)]
    [Unicode(false)]
    public string? Nombre { get; set; }

    [Column("descripcion")]
    [StringLength(400)]
    [Unicode(false)]
    public string? Descripcion { get; set; }

    [Column("fecha")]
    public DateOnly? Fecha { get; set; }

    [Column("hora")]
    public TimeOnly? Hora { get; set; }

    public virtual ClasePresencial? ClasePresencial { get; set; }

    public virtual ClaseVirtual? ClaseVirtual { get; set; }

    [ForeignKey("IdCursoSincronico")]
    public virtual CursoSincronico CursoSincronico{ get; set; } = null!;

    [ForeignKey("IdProfesor")]
    public virtual Profesor Profesor { get; set; } = null!;
}
