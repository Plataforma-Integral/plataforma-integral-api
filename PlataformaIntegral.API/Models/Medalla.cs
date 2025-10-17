using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PlataformaIntegral.API.Models;

[Table("medalla")]
public partial class Medalla
{
    [Key]
    [Column("id_medalla")]
    public int IdMedalla { get; set; }

    [Column("id_torneo")]
    public int? IdTorneo { get; set; }

    [Column("id_nivel_medalla")]
    public int? IdNivelMedalla { get; set; }

    [Column("nombre")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Nombre { get; set; }

    [Column("descripcion")]
    [StringLength(300)]
    [Unicode(false)]
    public string? Descripcion { get; set; }

    [Column("url_imagen")]
    [StringLength(300)]
    [Unicode(false)]
    public string? UrlImagen { get; set; }

    [InverseProperty("IdMedallaNavigation")]
    public virtual ICollection<EstudianteMedalla> EstudianteMedallas { get; set; } = new List<EstudianteMedalla>();

    [ForeignKey("IdNivelMedalla")]
    [InverseProperty("Medallas")]
    public virtual NivelMedalla? IdNivelMedallaNavigation { get; set; }

    [ForeignKey("IdTorneo")]
    [InverseProperty("Medallas")]
    public virtual Torneo? IdTorneoNavigation { get; set; }
}
