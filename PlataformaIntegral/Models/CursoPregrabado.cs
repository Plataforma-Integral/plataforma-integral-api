using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PlataformaIntegral.Models;

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

    [InverseProperty("IdCursoPregrabadoNavigation")]
    public virtual ICollection<Capitulo> Capitulos { get; set; } = new List<Capitulo>();

    [InverseProperty("IdCursoPregrabadoNavigation")]
    public virtual ICollection<Examan> Examen { get; set; } = new List<Examan>();

    [ForeignKey("IdCurso")]
    [InverseProperty("CursoPregrabado")]
    public virtual Curso IdCursoNavigation { get; set; } = null!;
}
