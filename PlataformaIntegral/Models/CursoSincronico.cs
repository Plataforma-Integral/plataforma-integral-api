using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PlataformaIntegral.Models;

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

    [InverseProperty("IdCursoSincronicoNavigation")]
    public virtual ICollection<Clase> Clases { get; set; } = new List<Clase>();

    [ForeignKey("IdCurso")]
    [InverseProperty("CursoSincronico")]
    public virtual Curso IdCursoNavigation { get; set; } = null!;

    [ForeignKey("IdModalidad")]
    [InverseProperty("CursoSincronicos")]
    public virtual ModalidadSincronico? IdModalidadNavigation { get; set; }
}
