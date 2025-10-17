using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PlataformaIntegral.API.Models;

[Table("profesor")]
public partial class Profesor
{
    [Key]
    [Column("id_usuario")]
    public int IdUsuario { get; set; }

    [Column("calificacion", TypeName = "decimal(3, 2)")]
    public decimal? Calificacion { get; set; }

    [Column("estado_verificacion")]
    public bool? EstadoVerificacion { get; set; }

    [Column("disponibilidad")]
    [StringLength(120)]
    [Unicode(false)]
    public string? Disponibilidad { get; set; }

    [InverseProperty("IdProfesorNavigation")]
    public virtual ICollection<Clase> Clases { get; set; } = new List<Clase>();

    [ForeignKey("IdUsuario")]
    [InverseProperty("Profesor")]
    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;

    [InverseProperty("IdProfesorNavigation")]
    public virtual ICollection<ProfesorCurso> ProfesorCursos { get; set; } = new List<ProfesorCurso>();

    [InverseProperty("IdProfesorNavigation")]
    public virtual ICollection<ReseñaProfesor> ReseñaProfesores { get; set; } = new List<ReseñaProfesor>();

    [ForeignKey("IdProfesor")]
    [InverseProperty("IdProfesors")]
    public virtual ICollection<Categoria> IdCategoria { get; set; } = new List<Categoria>();
}
