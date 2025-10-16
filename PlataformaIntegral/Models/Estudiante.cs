using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PlataformaIntegral.Models;

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

    [InverseProperty("IdUsuarioNavigation")]
    public virtual ICollection<EstudianteMedalla> EstudianteMedallas { get; set; } = new List<EstudianteMedalla>();

    [InverseProperty("IdEstudianteNavigation")]
    public virtual ICollection<EstudianteProgreso> EstudianteProgresos { get; set; } = new List<EstudianteProgreso>();

    [ForeignKey("IdUsuario")]
    [InverseProperty("Estudiante")]
    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;

    [InverseProperty("IdEstudianteNavigation")]
    public virtual ICollection<ReseñaCurso> ReseñaCursos { get; set; } = new List<ReseñaCurso>();

    [InverseProperty("IdEstudianteNavigation")]
    public virtual ICollection<ReseñaProfesor> ReseñaProfesors { get; set; } = new List<ReseñaProfesor>();

    [ForeignKey("IdEstudiante")]
    [InverseProperty("IdEstudiantes")]
    public virtual ICollection<Categorium> IdCategoria { get; set; } = new List<Categorium>();

    [ForeignKey("IdEstudiante")]
    [InverseProperty("IdEstudiantes")]
    public virtual ICollection<Certificado> IdCertificados { get; set; } = new List<Certificado>();

    [ForeignKey("IdEstudiante")]
    [InverseProperty("IdEstudiantes")]
    public virtual ICollection<Torneo> IdTorneos { get; set; } = new List<Torneo>();
}
