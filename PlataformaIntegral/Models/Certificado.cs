using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PlataformaIntegral.Models;

[Table("certificado")]
public partial class Certificado
{
    [Key]
    [Column("id_recurso")]
    public int IdRecurso { get; set; }

    [Column("id_curso")]
    public int IdCurso { get; set; }

    [ForeignKey("IdCurso")]
    [InverseProperty("Certificados")]
    public virtual Curso IdCursoNavigation { get; set; } = null!;

    [ForeignKey("IdRecurso")]
    [InverseProperty("Certificado")]
    public virtual Recurso IdRecursoNavigation { get; set; } = null!;

    [ForeignKey("IdCertificado")]
    [InverseProperty("IdCertificados")]
    public virtual ICollection<Estudiante> IdEstudiantes { get; set; } = new List<Estudiante>();
}
