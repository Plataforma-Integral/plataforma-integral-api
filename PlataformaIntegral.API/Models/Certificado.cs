using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PlataformaIntegral.API.Models;

[Table("certificado")]
public partial class Certificado
{
    [Key]
    [Column("id_recurso")]
    public int IdRecurso { get; set; }

    [Column("id_curso")]
    public int IdCurso { get; set; }

    [ForeignKey("IdCurso")]
    public virtual Curso Curso { get; set; } = null!;

    [ForeignKey("IdRecurso")]
    public virtual Recurso Recurso { get; set; } = null!;

    [ForeignKey("IdCertificado")]
    public virtual ICollection<Estudiante> Estudiantes { get; set; } = new List<Estudiante>();
}
