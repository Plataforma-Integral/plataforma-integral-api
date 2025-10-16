using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PlataformaIntegral.Models;

[Table("cuestionario")]
public partial class Cuestionario
{
    [Key]
    [Column("id_recurso")]
    public int IdRecurso { get; set; }

    [Column("id_capitulo")]
    public int IdCapitulo { get; set; }

    [Column("numero_orden")]
    public int? NumeroOrden { get; set; }

    [ForeignKey("IdCapitulo")]
    [InverseProperty("Cuestionarios")]
    public virtual Capitulo IdCapituloNavigation { get; set; } = null!;

    [ForeignKey("IdRecurso")]
    [InverseProperty("Cuestionario")]
    public virtual Recurso IdRecursoNavigation { get; set; } = null!;
}
