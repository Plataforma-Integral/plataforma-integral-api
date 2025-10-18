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

    public virtual Certificado? Certificado { get; set; }

    public virtual Cuestionario? Cuestionario { get; set; }

    public virtual Documento? Documento { get; set; }

    public virtual ICollection<EstudianteProgreso> EstudianteProgresos { get; set; } = new List<EstudianteProgreso>();

    public virtual Examen? Examen { get; set; }

    public virtual Video? Video { get; set; }
}
