using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;
using Microsoft.EntityFrameworkCore;
using Minio.DataModel.ILM;

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

    [Column("duracion_segundos")]
    public decimal DuracionSegundos { get; set; }

    [Column("peso_bytes")]
    public long PesoBytes { get; set; }

    [Column("miniatura_url")]
    [StringLength(200)]
    [Unicode(false)]
    public string? MiniaturaUrl { get; set; }

    public virtual ICollection<Documento> Documentos { get; set; } = new List<Documento>();

    [ForeignKey("IdCapitulo")]
    public virtual Capitulo Capitulo { get; set; } = null!;

    [ForeignKey("IdRecurso")]
    public virtual Recurso Recurso { get; set; } = null!;
}
