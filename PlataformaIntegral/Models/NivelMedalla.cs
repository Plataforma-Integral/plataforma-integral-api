using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PlataformaIntegral.Models;

[Table("nivel_medalla")]
[Index("Nombre", Name = "UQ__nivel_me__72AFBCC635998279", IsUnique = true)]
public partial class NivelMedalla
{
    [Key]
    [Column("id_nivel_medalla")]
    public int IdNivelMedalla { get; set; }

    [Column("nombre")]
    [StringLength(50)]
    [Unicode(false)]
    public string Nombre { get; set; } = null!;

    [InverseProperty("IdNivelMedallaNavigation")]
    public virtual ICollection<Medalla> Medallas { get; set; } = new List<Medalla>();
}
