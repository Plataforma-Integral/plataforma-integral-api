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

    [InverseProperty("IdClaseNavigation")]
    public virtual ClasePresencial? ClasePresencial { get; set; }

    [InverseProperty("IdClaseNavigation")]
    public virtual ClaseVirtual? ClaseVirtual { get; set; }

    [ForeignKey("IdCursoSincronico")]
    [InverseProperty("Clases")]
    public virtual CursoSincronico IdCursoSincronicoNavigation { get; set; } = null!;

    [ForeignKey("IdProfesor")]
    [InverseProperty("Clases")]
    public virtual Profesor IdProfesorNavigation { get; set; } = null!;
}
