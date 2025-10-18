using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PlataformaIntegral.API.Models;

[Table("video")]
public partial class Video
{
    [Key]
    [Column("id_recurso")]
    public int IdRecurso { get; set; }

    [Column("id_capitulo")]
    public int IdCapitulo { get; set; }

    [Column("descripcion")]
    [StringLength(300)]
    [Unicode(false)]
    public string? Descripcion { get; set; }

    [Column("numero_orden")]
    public int? NumeroOrden { get; set; }

    [Column("tipo")]
    [StringLength(50)]
    [Unicode(false)]
    public string? Tipo { get; set; }

    public virtual ICollection<Documento> Documentos { get; set; } = new List<Documento>();

    [ForeignKey("IdCapitulo")]
    public virtual Capitulo Capitulo { get; set; } = null!;

    [ForeignKey("IdRecurso")]
    public virtual Recurso Recurso { get; set; } = null!;
}
