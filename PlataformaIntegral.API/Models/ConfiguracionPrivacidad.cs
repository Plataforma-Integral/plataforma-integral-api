using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PlataformaIntegral.API.Models;

[Table("configuracion_privacidad")]
public partial class ConfiguracionPrivacidad
{
    [Key]
    [Column("id_usuario")]
    public int IdUsuario { get; set; }

    [Column("mostrar_email")]
    public bool? MostrarEmail { get; set; }

    [Column("mostrar_telefono")]
    public bool? MostrarTelefono { get; set; }

    [Column("mostrar_nombre")]
    public bool? MostrarNombre { get; set; }

    [Column("mostrar_fecha_nacimiento")]
    public bool? MostrarFechaNacimiento { get; set; }

    [Column("mostrar_medallas")]
    public bool? MostrarMedallas { get; set; }

    [ForeignKey("IdUsuario")]
    [InverseProperty("ConfiguracionPrivacidad")]
    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
