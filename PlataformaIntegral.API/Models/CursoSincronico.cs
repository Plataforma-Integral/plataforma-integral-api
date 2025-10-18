using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PlataformaIntegral.API.Models;

[Table("curso_sincronico")]
public partial class CursoSincronico
{
    [Key]
    [Column("id_curso")]
    public int IdCurso { get; set; }

    [Column("id_modalidad")]
    public int? IdModalidad { get; set; }

    [Column("fecha_inicio")]
    public DateOnly? FechaInicio { get; set; }

    [Column("duracion")]
    public int? Duracion { get; set; }

    public virtual ICollection<Clase> Clases { get; set; } = new List<Clase>();

    [ForeignKey("IdCurso")]
    public virtual Curso Curso { get; set; } = null!;

    [ForeignKey("IdModalidad")]
    public virtual ModalidadSincronico? Modalidad { get; set; }
}
