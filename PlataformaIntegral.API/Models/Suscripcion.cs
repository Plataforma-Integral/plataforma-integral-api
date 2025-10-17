using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PlataformaIntegral.API.Models;

[Table("suscripcion")]
public partial class Suscripcion
{
    [Key]
    [Column("id_suscripcion")]
    public int IdSuscripcion { get; set; }

    [Column("id_suscripcion_tipo")]
    public int? IdSuscripcionTipo { get; set; }

    [Column("id_usuario")]
    public int? IdUsuario { get; set; }

    [Column("fecha_inicio")]
    public DateOnly? FechaInicio { get; set; }

    [Column("fecha_fin")]
    public DateOnly? FechaFin { get; set; }

    [Column("id_estado_suscripcion")]
    public int? IdEstadoSuscripcion { get; set; }

    [ForeignKey("IdEstadoSuscripcion")]
    [InverseProperty("Suscripcions")]
    public virtual EstadoSuscripcion? IdEstadoSuscripcionNavigation { get; set; }

    [ForeignKey("IdSuscripcionTipo")]
    [InverseProperty("Suscripcions")]
    public virtual SuscripcionTipo? IdSuscripcionTipoNavigation { get; set; }

    [ForeignKey("IdUsuario")]
    [InverseProperty("Suscripcions")]
    public virtual Usuario? IdUsuarioNavigation { get; set; }
}
