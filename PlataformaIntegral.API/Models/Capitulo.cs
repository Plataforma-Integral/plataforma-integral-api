using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using PlataformaIntegral.API.Models;

namespace PlataformaIntegral.API.Models;

[Table("capitulo")]
public partial class Capitulo
{
    [Key]
    [Column("id_capitulo")]
    public int IdCapitulo { get; set; }

    [Column("id_curso_pregrabado")]
    public int IdCursoPregrabado { get; set; }

    [Column("nombre")]
    [StringLength(120)]
    [Unicode(false)]
    public string? Nombre { get; set; }

    [Column("descripcion")]
    [StringLength(400)]
    [Unicode(false)]
    public string? Descripcion { get; set; }

    [Column("numero_orden")]
    public int? NumeroOrden { get; set; }

    [InverseProperty("IdCapituloNavigation")]
    public virtual ICollection<Cuestionario> Cuestionarios { get; set; } = new List<Cuestionario>();

    [ForeignKey("IdCursoPregrabado")]
    [InverseProperty("Capitulos")]
    public virtual CursoPregrabado IdCursoPregrabadoNavigation { get; set; } = null!;

    [InverseProperty("IdCapituloNavigation")]
    public virtual ICollection<Video> Videos { get; set; } = new List<Video>();
}
