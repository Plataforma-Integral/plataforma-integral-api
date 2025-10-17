using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PlataformaIntegral.API.Models;

[Table("documento")]
public partial class Documento
{
    [Key]
    [Column("id_recurso")]
    public int IdRecurso { get; set; }

    [Column("id_video")]
    public int? IdVideo { get; set; }

    [ForeignKey("IdRecurso")]
    [InverseProperty("Documento")]
    public virtual Recurso IdRecursoNavigation { get; set; } = null!;

    [ForeignKey("IdVideo")]
    [InverseProperty("Documentos")]
    public virtual Video? IdVideoNavigation { get; set; }
}
