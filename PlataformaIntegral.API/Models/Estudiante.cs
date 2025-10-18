using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PlataformaIntegral.API.Models;

[Table("estudiante")]
public partial class Estudiante
{
    [Key]
    [Column("id_usuario")]
    public int IdUsuario { get; set; }

    [Column("educacion")]
    [StringLength(120)]
    [Unicode(false)]
    public string? Educacion { get; set; }

    [Column("ultimo_login", TypeName = "datetime")]
    public DateTime? UltimoLogin { get; set; }

    [Column("puntos")]
    public int? Puntos { get; set; }

    public virtual ICollection<EstudianteMedalla> EstudianteMedallas { get; set; } = new List<EstudianteMedalla>();

    public virtual ICollection<EstudianteProgreso> EstudianteProgresos { get; set; } = new List<EstudianteProgreso>();

    [ForeignKey("IdUsuario")]
    public virtual Usuario Usuario { get; set; } = null!;

    public virtual ICollection<ReseñaCurso> ReseñaCursos { get; set; } = new List<ReseñaCurso>();

    public virtual ICollection<ReseñaProfesor> ReseñaProfesores { get; set; } = new List<ReseñaProfesor>();

    [ForeignKey("Estudiante")]
    public virtual ICollection<Categoria> Categorias { get; set; } = new List<Categoria>();

    [ForeignKey("Estudiante")]
    public virtual ICollection<Certificado> Certificados { get; set; } = new List<Certificado>();

    [ForeignKey("Estudiante")]
    public virtual ICollection<Torneo> Torneos { get; set; } = new List<Torneo>();
}
