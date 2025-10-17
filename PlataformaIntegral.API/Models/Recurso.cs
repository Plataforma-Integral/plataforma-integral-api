using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PlataformaIntegral.API.Models;

[Table("recurso")]
public partial class Recurso
{
    [Key]
    [Column("id_recurso")]
    public int IdRecurso { get; set; }

    [Column("nombre")]
    [StringLength(120)]
    [Unicode(false)]
    public string? Nombre { get; set; }

    [Column("url")]
    [StringLength(300)]
    [Unicode(false)]
    public string? Url { get; set; }

    [InverseProperty("IdRecursoNavigation")]
    public virtual Certificado? Certificado { get; set; }

    [InverseProperty("IdRecursoNavigation")]
    public virtual Cuestionario? Cuestionario { get; set; }

    [InverseProperty("IdRecursoNavigation")]
    public virtual Documento? Documento { get; set; }

    [InverseProperty("IdRecursoNavigation")]
    public virtual ICollection<EstudianteProgreso> EstudianteProgresos { get; set; } = new List<EstudianteProgreso>();

    [InverseProperty("IdRecursoNavigation")]
    public virtual Examan? Examan { get; set; }

    [InverseProperty("IdRecursoNavigation")]
    public virtual Video? Video { get; set; }
}
