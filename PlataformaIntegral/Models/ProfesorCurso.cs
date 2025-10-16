using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PlataformaIntegral.Models;

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
    [InverseProperty("ProfesorCursos")]
    public virtual Curso IdCursoNavigation { get; set; } = null!;

    [ForeignKey("IdProfesor")]
    [InverseProperty("ProfesorCursos")]
    public virtual Profesor IdProfesorNavigation { get; set; } = null!;

    [ForeignKey("IdTipoRol")]
    [InverseProperty("ProfesorCursos")]
    public virtual TipoRol? IdTipoRolNavigation { get; set; }
}
