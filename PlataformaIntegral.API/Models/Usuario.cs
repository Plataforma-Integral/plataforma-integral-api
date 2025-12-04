using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PlataformaIntegral.API.Models;

[Table("usuario")]
public partial class Usuario
{
    [Key]
    [Column("id_usuario")]
    public int IdUsuario { get; set; }

    [Column("nombre")]
    [StringLength(80)]
    [Unicode(false)]
    public string Nombre { get; set; } = null!;

    [Column("apellido")]
    [StringLength(80)]
    [Unicode(false)]
    public string Apellido { get; set; } = null!;

    [Column("seudonimo")]
    [StringLength(50)]
    [Unicode(false)]
    public string? Seudonimo { get; set; }

    [Column("biografia")]
    [StringLength(400)]
    [Unicode(false)]
    public string? Biografia { get; set; }

    [Column("telefono")]
    [StringLength(20)]
    [Unicode(false)]
    public string? Telefono { get; set; }

    [Column("genero")]
    [StringLength(1)]
    [Unicode(false)]
    public string? Genero { get; set; }

    [Column("fecha_nacimiento")]
    public DateOnly? FechaNacimiento { get; set; }

    [Column("id_tipo_usuario")]
    public int? IdTipoUsuario { get; set; }

    [Column("id_pais")]
    public int? IdPais { get; set; }

    [Column("departamento")]
    [StringLength(80)]
    [Unicode(false)]
    public string? Departamento { get; set; }

    [Column("ciudad")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Ciudad { get; set; }

    [Column("nivel_educativo")]
    [StringLength(100)]
    [Unicode(false)]
    public string? NivelEducativo { get; set; }

    [Column("fecha_registro", TypeName = "datetime")]
    public DateTime? FechaRegistro { get; set; }

    [Column("imagen_bucket_name")]
    [StringLength(200)]
    [Unicode(false)]
    public string? ImagenBucketName { get; set; }

    public virtual Administrador? Administrador { get; set; }
    public virtual ConfiguracionPrivacidad? ConfiguracionPrivacidad { get; set; }
    public virtual ICollection<Credencial> Credenciales { get; set; } = new List<Credencial>();
    public virtual Estudiante? Estudiante { get; set; }

    [ForeignKey("IdPais")]
    public virtual Pais? Pais { get; set; }

    [ForeignKey("IdTipoUsuario")]
    public virtual TipoUsuario? TipoUsuario { get; set; }

    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();
    public virtual Profesor? Profesor { get; set; }
    public virtual ICollection<Recibo> Recibos { get; set; } = new List<Recibo>();
    public virtual ICollection<Suscripcion> Suscripciones { get; set; } = new List<Suscripcion>();
}
