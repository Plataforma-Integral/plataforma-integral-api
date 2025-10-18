using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PlataformaIntegral.API.Models;

[Table("modalidad_sincronico")]
[Index("Nombre", Name = "UQ__modalida__72AFBCC64646F5C4", IsUnique = true)]
public partial class ModalidadSincronico
{
    [Key]
    [Column("id_modalidad")]
    public int IdModalidad { get; set; }

    [Column("nombre")]
    [StringLength(50)]
    [Unicode(false)]
    public string Nombre { get; set; } = null!;

    public virtual ICollection<CursoSincronico> CursoSincronicos { get; set; } = new List<CursoSincronico>();
}
