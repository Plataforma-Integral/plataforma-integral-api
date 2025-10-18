using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PlataformaIntegral.API.Models;

[PrimaryKey("IdProfesor", "IdCurso")]
[Table("profesor_curso")]
public partial class ProfesorCurso
{
    [Key]
    [Column("id_profesor")]
    public int IdProfesor { get; set; }

    [Key]
    [Column("id_curso")]
    public int IdCurso { get; set; }

    [Column("id_tipo_rol")]
    public int? IdTipoRol { get; set; }

    [ForeignKey("IdCurso")]
    public virtual Curso Curso{ get; set; } = null!;

    [ForeignKey("IdProfesor")]
    public virtual Profesor Profesor { get; set; } = null!;

    [ForeignKey("IdTipoRol")]
    public virtual TipoRol? TipoRol { get; set; }
}
