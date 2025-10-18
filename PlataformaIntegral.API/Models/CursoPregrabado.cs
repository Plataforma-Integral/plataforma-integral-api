using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PlataformaIntegral.API.Models;

[Table("curso_pregrabado")]
public partial class CursoPregrabado
{
    [Key]
    [Column("id_curso")]
    public int IdCurso { get; set; }

    [Column("id_certificado")]
    public int? IdCertificado { get; set; }

    [Column("precio_puntos")]
    public int? PrecioPuntos { get; set; }

    [Column("url_portada")]
    [StringLength(300)]
    [Unicode(false)]
    public string? UrlPortada { get; set; }

    [Column("url_video")]
    [StringLength(300)]
    [Unicode(false)]
    public string? UrlVideo { get; set; }

    public virtual ICollection<Capitulo> Capitulos { get; set; } = new List<Capitulo>();

    public virtual ICollection<Examen> Examen { get; set; } = new List<Examen>();

    [ForeignKey("IdCurso")]
    public virtual Curso Curso { get; set; } = null!;
}
