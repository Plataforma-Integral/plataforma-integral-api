using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PlataformaIntegral.API.Models;

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
    public virtual Capitulo Capitulo{ get; set; } = null!;

    [ForeignKey("IdRecurso")]
    public virtual Recurso Recurso { get; set; } = null!;
}
