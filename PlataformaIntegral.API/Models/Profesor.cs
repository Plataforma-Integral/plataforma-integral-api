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

    public virtual ICollection<Clase> Clases { get; set; } = new List<Clase>();

    [ForeignKey("IdUsuario")]
    public virtual Usuario Usuario { get; set; } = null!;

    public virtual ICollection<ProfesorCurso> ProfesorCursos { get; set; } = new List<ProfesorCurso>();

    public virtual ICollection<ResenaProfesor> ReseñasProfesor { get; set; } = new List<ResenaProfesor>();

    [ForeignKey("IdProfesor")]
    public virtual ICollection<Categoria> Categorias { get; set; } = new List<Categoria>();
}
