using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PlataformaIntegral.API.Models;

public partial class PlataformaIntegralContext : DbContext
{
    public PlataformaIntegralContext()
    {
    }

    public PlataformaIntegralContext(DbContextOptions<PlataformaIntegralContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Administrador> Administradores { get; set; }

    public virtual DbSet<AdministradorTorneo> AdministradorTorneos { get; set; }

    public virtual DbSet<Capitulo> Capitulos { get; set; }

    public virtual DbSet<Categoria> Categoria { get; set; }

    public virtual DbSet<Certificado> Certificados { get; set; }

    public virtual DbSet<Clase> Clases { get; set; }

    public virtual DbSet<ClasePresencial> ClasePresenciales { get; set; }

    public virtual DbSet<ClaseVirtual> ClaseVirtuales { get; set; }

    public virtual DbSet<ConfiguracionPrivacidad> ConfiguracionesPrivacidad { get; set; }

    public virtual DbSet<Credencial> Credenciales { get; set; }

    public virtual DbSet<Cuestionario> Cuestionarios { get; set; }

    public virtual DbSet<Curso> Cursos { get; set; }

    public virtual DbSet<CursoPregrabado> CursoPregrabados { get; set; }

    public virtual DbSet<CursoSincronico> CursoSincronicos { get; set; }

    public virtual DbSet<Documento> Documentos { get; set; }

    public virtual DbSet<EstadoPago> EstadoPagos { get; set; }

    public virtual DbSet<EstadoSuscripcion> EstadoSuscripcions { get; set; }

    public virtual DbSet<Estudiante> Estudiantes { get; set; }

    public virtual DbSet<EstudianteMedalla> EstudianteMedallas { get; set; }

    public virtual DbSet<EstudianteProgreso> EstudianteProgresos { get; set; }

    public virtual DbSet<Examan> Examen { get; set; }

    public virtual DbSet<Medalla> Medallas { get; set; }

    public virtual DbSet<MetodoPago> MetodoPagos { get; set; }

    public virtual DbSet<ModalidadSincronico> ModalidadSincronicos { get; set; }

    public virtual DbSet<NivelMedalla> NivelMedallas { get; set; }

    public virtual DbSet<Pago> Pagos { get; set; }

    public virtual DbSet<Pais> Paises { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<Profesor> Profesores { get; set; }

    public virtual DbSet<ProfesorCurso> ProfesorCursos { get; set; }

    public virtual DbSet<Recibo> Recibos { get; set; }

    public virtual DbSet<Recurso> Recursos { get; set; }

    public virtual DbSet<ReseñaCurso> ReseñaCursos { get; set; }

    public virtual DbSet<ReseñaProfesor> ReseñaProfesores { get; set; }

    public virtual DbSet<Suscripcion> Suscripciones { get; set; }

    public virtual DbSet<SuscripcionTipo> SuscripcionesTipo { get; set; }

    public virtual DbSet<TipoMonedum> TipoMoneda { get; set; }

    public virtual DbSet<TipoRol> TipoRoles { get; set; }

    public virtual DbSet<TipoUsuario> TipoUsuarios { get; set; }

    public virtual DbSet<Torneo> Torneos { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<Video> Videos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=PlataformaIntegralDB");


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Administrador>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__administ__4E3E04AD893284E9");

            entity.Property(e => e.IdUsuario).ValueGeneratedNever();

            entity.HasOne(d => d.IdUsuarioNavigation).WithOne(p => p.Administrador).HasConstraintName("FK_administrador_usuario");
        });

        modelBuilder.Entity<AdministradorTorneo>(entity =>
        {
            entity.HasKey(e => new { e.IdAdministrador, e.IdTorneo }).HasName("PK__administ__925340057C3F9EBE");

            entity.HasOne(d => d.IdAdministradorNavigation).WithMany(p => p.AdministradorTorneos).HasConstraintName("FK_at_administrador");

            entity.HasOne(d => d.IdTipoRolNavigation).WithMany(p => p.AdministradorTorneos).HasConstraintName("FK_at_tipo_rol");

            entity.HasOne(d => d.IdTorneoNavigation).WithMany(p => p.AdministradorTorneos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_at_torneo");
        });

        modelBuilder.Entity<Capitulo>(entity =>
        {
            entity.HasKey(e => e.IdCapitulo).HasName("PK__capitulo__5ABB2D5998346CE5");

            entity.HasOne(d => d.IdCursoPregrabadoNavigation).WithMany(p => p.Capitulos).HasConstraintName("FK_capitulo_curso_pregrabado");
        });

        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.IdCategoria).HasName("PK__categori__CD54BC5ABF7A6675");

            entity.HasOne(d => d.IdSuperCategoriaNavigation).WithMany(p => p.InverseIdSuperCategoriaNavigation).HasConstraintName("FK_categoria_super_categoria");
        });

        modelBuilder.Entity<Certificado>(entity =>
        {
            entity.HasKey(e => e.IdRecurso).HasName("PK__certific__2B386DE45ECE8B1A");

            entity.Property(e => e.IdRecurso).ValueGeneratedNever();

            entity.HasOne(d => d.IdCursoNavigation).WithMany(p => p.Certificados)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_certificado_curso");

            entity.HasOne(d => d.IdRecursoNavigation).WithOne(p => p.Certificado).HasConstraintName("FK_certificado_recurso");
        });

        modelBuilder.Entity<Clase>(entity =>
        {
            entity.HasKey(e => e.IdClase).HasName("PK__clase__2352EEDBD40CA134");

            entity.HasOne(d => d.IdCursoSincronicoNavigation).WithMany(p => p.Clases)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_clase_curso_sincronico");

            entity.HasOne(d => d.IdProfesorNavigation).WithMany(p => p.Clases)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_clase_profesor");
        });

        modelBuilder.Entity<ClasePresencial>(entity =>
        {
            entity.HasKey(e => e.IdClase).HasName("PK__clase_pr__2352EEDB8E301AB7");

            entity.Property(e => e.IdClase).ValueGeneratedNever();

            entity.HasOne(d => d.IdClaseNavigation).WithOne(p => p.ClasePresencial).HasConstraintName("FK_clase_presencial_clase");
        });

        modelBuilder.Entity<ClaseVirtual>(entity =>
        {
            entity.HasKey(e => e.IdClase).HasName("PK__clase_vi__2352EEDB4883FD9E");

            entity.Property(e => e.IdClase).ValueGeneratedNever();

            entity.HasOne(d => d.IdClaseNavigation).WithOne(p => p.ClaseVirtual).HasConstraintName("FK_clase_virtual_clase");
        });

        modelBuilder.Entity<ConfiguracionPrivacidad>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__configur__4E3E04AD7A525808");

            entity.Property(e => e.IdUsuario).ValueGeneratedNever();
            entity.Property(e => e.MostrarEmail).HasDefaultValue(true);
            entity.Property(e => e.MostrarFechaNacimiento).HasDefaultValue(false);
            entity.Property(e => e.MostrarMedallas).HasDefaultValue(true);
            entity.Property(e => e.MostrarNombre).HasDefaultValue(true);
            entity.Property(e => e.MostrarTelefono).HasDefaultValue(false);

            entity.HasOne(d => d.IdUsuarioNavigation).WithOne(p => p.ConfiguracionPrivacidad).HasConstraintName("FK_configuracion_usuario");
        });

        modelBuilder.Entity<Credencial>(entity =>
        {
            entity.HasKey(e => e.IdCredencial).HasName("PK__credenci__E81A3BAA3E569366");

            entity.Property(e => e.FechaCreacion).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Credenciales).HasConstraintName("FK_credencial_usuario");
        });

        modelBuilder.Entity<Cuestionario>(entity =>
        {
            entity.HasKey(e => e.IdRecurso).HasName("PK__cuestion__2B386DE4A62CC9EE");

            entity.Property(e => e.IdRecurso).ValueGeneratedNever();

            entity.HasOne(d => d.IdCapituloNavigation).WithMany(p => p.Cuestionarios)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_cuestionario_capitulo");

            entity.HasOne(d => d.IdRecursoNavigation).WithOne(p => p.Cuestionario).HasConstraintName("FK_cuestionario_recurso");
        });

        modelBuilder.Entity<Curso>(entity =>
        {
            entity.HasKey(e => e.IdProducto).HasName("PK__curso__FF341C0D16F5C272");

            entity.Property(e => e.IdProducto).ValueGeneratedNever();

            entity.HasOne(d => d.IdProductoNavigation).WithOne(p => p.Curso).HasConstraintName("FK_curso_producto");

            entity.HasMany(d => d.IdCategoria).WithMany(p => p.IdCursos)
                .UsingEntity<Dictionary<string, object>>(
                    "CursoCategorium",
                    r => r.HasOne<Categoria>().WithMany()
                        .HasForeignKey("IdCategoria")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_cc_categoria"),
                    l => l.HasOne<Curso>().WithMany()
                        .HasForeignKey("IdCurso")
                        .HasConstraintName("FK_cc_curso"),
                    j =>
                    {
                        j.HasKey("IdCurso", "IdCategoria").HasName("PK__curso_ca__E1EA3EC7318024B4");
                        j.ToTable("curso_categoria");
                        j.IndexerProperty<int>("IdCurso").HasColumnName("id_curso");
                        j.IndexerProperty<int>("IdCategoria").HasColumnName("id_categoria");
                    });
        });

        modelBuilder.Entity<CursoPregrabado>(entity =>
        {
            entity.HasKey(e => e.IdCurso).HasName("PK__curso_pr__5D3F750294C77A7A");

            entity.Property(e => e.IdCurso).ValueGeneratedNever();
            entity.Property(e => e.PrecioPuntos).HasDefaultValue(0);

            entity.HasOne(d => d.IdCursoNavigation).WithOne(p => p.CursoPregrabado).HasConstraintName("FK_cp_curso");
        });

        modelBuilder.Entity<CursoSincronico>(entity =>
        {
            entity.HasKey(e => e.IdCurso).HasName("PK__curso_si__5D3F7502EA649B0A");

            entity.Property(e => e.IdCurso).ValueGeneratedNever();

            entity.HasOne(d => d.IdCursoNavigation).WithOne(p => p.CursoSincronico).HasConstraintName("FK_cs_curso");

            entity.HasOne(d => d.IdModalidadNavigation).WithMany(p => p.CursoSincronicos).HasConstraintName("FK_cs_modalidad");
        });

        modelBuilder.Entity<Documento>(entity =>
        {
            entity.HasKey(e => e.IdRecurso).HasName("PK__document__2B386DE44364A476");

            entity.Property(e => e.IdRecurso).ValueGeneratedNever();

            entity.HasOne(d => d.IdRecursoNavigation).WithOne(p => p.Documento).HasConstraintName("FK_documento_recurso");

            entity.HasOne(d => d.IdVideoNavigation).WithMany(p => p.Documentos).HasConstraintName("FK_documento_video");
        });

        modelBuilder.Entity<EstadoPago>(entity =>
        {
            entity.HasKey(e => e.IdEstadoPago).HasName("PK__estado_p__7C6943D28867CC99");
        });

        modelBuilder.Entity<EstadoSuscripcion>(entity =>
        {
            entity.HasKey(e => e.IdEstadoSuscripcion).HasName("PK__estado_s__6D8351DFD489B418");
        });

        modelBuilder.Entity<Estudiante>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__estudian__4E3E04AD326BFEF7");

            entity.Property(e => e.IdUsuario).ValueGeneratedNever();
            entity.Property(e => e.Puntos).HasDefaultValue(0);

            entity.HasOne(d => d.IdUsuarioNavigation).WithOne(p => p.Estudiante).HasConstraintName("FK_estudiante_usuario");

            entity.HasMany(d => d.IdCategoria).WithMany(p => p.IdEstudiantes)
                .UsingEntity<Dictionary<string, object>>(
                    "EstudianteIntere",
                    r => r.HasOne<Categoria>().WithMany()
                        .HasForeignKey("IdCategoria")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_ei_interes_categoria"),
                    l => l.HasOne<Estudiante>().WithMany()
                        .HasForeignKey("IdEstudiante")
                        .HasConstraintName("FK_ei_interes_estudiante"),
                    j =>
                    {
                        j.HasKey("IdEstudiante", "IdCategoria").HasName("PK__estudian__5C673DF9362F804D");
                        j.ToTable("estudiante_interes");
                        j.IndexerProperty<int>("IdEstudiante").HasColumnName("id_estudiante");
                        j.IndexerProperty<int>("IdCategoria").HasColumnName("id_categoria");
                    });

            entity.HasMany(d => d.IdCertificados).WithMany(p => p.IdEstudiantes)
                .UsingEntity<Dictionary<string, object>>(
                    "EstudianteCertificado",
                    r => r.HasOne<Certificado>().WithMany()
                        .HasForeignKey("IdCertificado")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_ec_certificado"),
                    l => l.HasOne<Estudiante>().WithMany()
                        .HasForeignKey("IdEstudiante")
                        .HasConstraintName("FK_ec_estudiante"),
                    j =>
                    {
                        j.HasKey("IdEstudiante", "IdCertificado").HasName("PK__estudian__4BC85B2F9D3CECC5");
                        j.ToTable("estudiante_certificado");
                        j.IndexerProperty<int>("IdEstudiante").HasColumnName("id_estudiante");
                        j.IndexerProperty<int>("IdCertificado").HasColumnName("id_certificado");
                    });

            entity.HasMany(d => d.IdTorneos).WithMany(p => p.IdEstudiantes)
                .UsingEntity<Dictionary<string, object>>(
                    "EstudianteInscripcion",
                    r => r.HasOne<Torneo>().WithMany()
                        .HasForeignKey("IdTorneo")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_ei_torneo"),
                    l => l.HasOne<Estudiante>().WithMany()
                        .HasForeignKey("IdEstudiante")
                        .HasConstraintName("FK_ei_estudiante"),
                    j =>
                    {
                        j.HasKey("IdEstudiante", "IdTorneo").HasName("PK__estudian__7D0914933046201A");
                        j.ToTable("estudiante_inscripcion");
                        j.IndexerProperty<int>("IdEstudiante").HasColumnName("id_estudiante");
                        j.IndexerProperty<int>("IdTorneo").HasColumnName("id_torneo");
                    });
        });

        modelBuilder.Entity<EstudianteMedalla>(entity =>
        {
            entity.HasKey(e => new { e.IdUsuario, e.IdMedalla }).HasName("PK__estudian__6DE6D3481F777028");

            entity.Property(e => e.FechaOtorgada).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.IdMedallaNavigation).WithMany(p => p.EstudianteMedallas)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_em_medalla");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.EstudianteMedallas).HasConstraintName("FK_em_estudiante");
        });

        modelBuilder.Entity<EstudianteProgreso>(entity =>
        {
            entity.HasKey(e => new { e.IdEstudiante, e.IdRecurso }).HasName("PK__estudian__B201F0E26D3D4D79");

            entity.HasOne(d => d.IdEstudianteNavigation).WithMany(p => p.EstudianteProgresos).HasConstraintName("FK_ep_estudiante");

            entity.HasOne(d => d.IdRecursoNavigation).WithMany(p => p.EstudianteProgresos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ep_recurso");
        });

        modelBuilder.Entity<Examan>(entity =>
        {
            entity.HasKey(e => e.IdRecurso).HasName("PK__examen__2B386DE49AEE525E");

            entity.Property(e => e.IdRecurso).ValueGeneratedNever();

            entity.HasOne(d => d.IdCursoPregrabadoNavigation).WithMany(p => p.Examen).HasConstraintName("FK_examen_curso_pregrabado");

            entity.HasOne(d => d.IdRecursoNavigation).WithOne(p => p.Examan).HasConstraintName("FK_examen_recurso");
        });

        modelBuilder.Entity<Medalla>(entity =>
        {
            entity.HasKey(e => e.IdMedalla).HasName("PK__medalla__3D8D7E53DC272BF7");

            entity.HasOne(d => d.IdNivelMedallaNavigation).WithMany(p => p.Medallas).HasConstraintName("FK_medalla_nivel");

            entity.HasOne(d => d.IdTorneoNavigation).WithMany(p => p.Medallas).HasConstraintName("FK_medalla_torneo");
        });

        modelBuilder.Entity<MetodoPago>(entity =>
        {
            entity.HasKey(e => e.IdMetodoPago).HasName("PK__metodo_p__85BE0EBC8C781734");
        });

        modelBuilder.Entity<ModalidadSincronico>(entity =>
        {
            entity.HasKey(e => e.IdModalidad).HasName("PK__modalida__C4D7F0726876EE6C");
        });

        modelBuilder.Entity<NivelMedalla>(entity =>
        {
            entity.HasKey(e => e.IdNivelMedalla).HasName("PK__nivel_me__50FA9DC50BA6AD7C");
        });

        modelBuilder.Entity<Pago>(entity =>
        {
            entity.HasKey(e => e.IdPago).HasName("PK__pago__0941B074C20A764A");

            entity.Property(e => e.FechaPago).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.IdEstadoPagoNavigation).WithMany(p => p.Pagos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_pago_estado");

            entity.HasOne(d => d.IdMetodoPagoNavigation).WithMany(p => p.Pagos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_pago_metodo");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.Pagos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_pago_producto");

            entity.HasOne(d => d.IdReciboNavigation).WithMany(p => p.Pagos).HasConstraintName("FK_pago_recibo");

            entity.HasOne(d => d.IdTipoMonedaNavigation).WithMany(p => p.Pagos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_pago_moneda");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Pagos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_pago_usuario");
        });

        modelBuilder.Entity<Pais>(entity =>
        {
            entity.HasKey(e => e.IdPais).HasName("PK__pais__0941A3A7C4354F0A");

            entity.Property(e => e.CodigoIso).IsFixedLength();
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.IdProducto).HasName("PK__producto__FF341C0DD9CAFFC1");

            entity.Property(e => e.FechaCreacion).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<Profesor>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__profesor__4E3E04AD4DD94678");

            entity.Property(e => e.IdUsuario).ValueGeneratedNever();
            entity.Property(e => e.Calificacion).HasDefaultValue(0m);
            entity.Property(e => e.EstadoVerificacion).HasDefaultValue(false);

            entity.HasOne(d => d.IdUsuarioNavigation).WithOne(p => p.Profesor).HasConstraintName("FK_profesor_usuario");

            entity.HasMany(d => d.IdCategoria).WithMany(p => p.IdProfesors)
                .UsingEntity<Dictionary<string, object>>(
                    "ProfesorEspecialidad",
                    r => r.HasOne<Categoria>().WithMany()
                        .HasForeignKey("IdCategoria")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_pe_categoria"),
                    l => l.HasOne<Profesor>().WithMany()
                        .HasForeignKey("IdProfesor")
                        .HasConstraintName("FK_pe_profesor"),
                    j =>
                    {
                        j.HasKey("IdProfesor", "IdCategoria").HasName("PK__profesor__A94B9DD29F175119");
                        j.ToTable("profesor_especialidad");
                        j.IndexerProperty<int>("IdProfesor").HasColumnName("id_profesor");
                        j.IndexerProperty<int>("IdCategoria").HasColumnName("id_categoria");
                    });
        });

        modelBuilder.Entity<ProfesorCurso>(entity =>
        {
            entity.HasKey(e => new { e.IdProfesor, e.IdCurso }).HasName("PK__profesor__204D2147AD7CBF77");

            entity.HasOne(d => d.IdCursoNavigation).WithMany(p => p.ProfesorCursos).HasConstraintName("FK_pc_curso");

            entity.HasOne(d => d.IdProfesorNavigation).WithMany(p => p.ProfesorCursos).HasConstraintName("FK_pc_profesor");

            entity.HasOne(d => d.IdTipoRolNavigation).WithMany(p => p.ProfesorCursos).HasConstraintName("FK_pc_tipo_rol");
        });

        modelBuilder.Entity<Recibo>(entity =>
        {
            entity.HasKey(e => e.IdRecibo).HasName("PK__recibo__1F2CC1BA99E0CD30");

            entity.Property(e => e.FechaEmision).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.IdEstadoPagoNavigation).WithMany(p => p.Recibos).HasConstraintName("FK_recibo_estado");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Recibos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_recibo_usuario");
        });

        modelBuilder.Entity<Recurso>(entity =>
        {
            entity.HasKey(e => e.IdRecurso).HasName("PK__recurso__2B386DE4ACF6B739");
        });

        modelBuilder.Entity<ReseñaCurso>(entity =>
        {
            entity.HasKey(e => new { e.IdEstudiante, e.IdCurso }).HasName("PK__reseña_c__D561816C889DF7A4");

            entity.HasOne(d => d.IdCursoNavigation).WithMany(p => p.ReseñaCursos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_rc_curso");

            entity.HasOne(d => d.IdEstudianteNavigation).WithMany(p => p.ReseñaCursos).HasConstraintName("FK_rc_estudiante");
        });

        modelBuilder.Entity<ReseñaProfesor>(entity =>
        {
            entity.HasKey(e => new { e.IdEstudiante, e.IdProfesor }).HasName("PK__reseña_p__81EB9B5D05437026");

            entity.HasOne(d => d.IdEstudianteNavigation).WithMany(p => p.ReseñaProfesores).HasConstraintName("FK_rp_estudiante");

            entity.HasOne(d => d.IdProfesorNavigation).WithMany(p => p.ReseñaProfesores)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_rp_profesor");
        });

        modelBuilder.Entity<Suscripcion>(entity =>
        {
            entity.HasKey(e => e.IdSuscripcion).HasName("PK__suscripc__4E8926BB7D9E2935");

            entity.HasOne(d => d.IdEstadoSuscripcionNavigation).WithMany(p => p.Suscripciones).HasConstraintName("FK_suscripcion_estado");

            entity.HasOne(d => d.IdSuscripcionTipoNavigation).WithMany(p => p.Suscripciones).HasConstraintName("FK_suscripcion_tipo");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Suscripciones).HasConstraintName("FK_suscripcion_usuario");
        });

        modelBuilder.Entity<SuscripcionTipo>(entity =>
        {
            entity.HasKey(e => e.IdProducto).HasName("PK__suscripc__FF341C0D08D0C6DA");

            entity.Property(e => e.IdProducto).ValueGeneratedNever();

            entity.HasOne(d => d.IdProductoNavigation).WithOne(p => p.SuscripcionTipo).HasConstraintName("FK_st_producto");

            entity.HasMany(d => d.IdCursos).WithMany(p => p.IdSuscripcionTipos)
                .UsingEntity<Dictionary<string, object>>(
                    "SuscripcionCurso",
                    r => r.HasOne<Curso>().WithMany()
                        .HasForeignKey("IdCurso")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_sc_curso"),
                    l => l.HasOne<SuscripcionTipo>().WithMany()
                        .HasForeignKey("IdSuscripcionTipo")
                        .HasConstraintName("FK_sc_tipo"),
                    j =>
                    {
                        j.HasKey("IdSuscripcionTipo", "IdCurso").HasName("PK__suscripc__72A57FF528B326C5");
                        j.ToTable("suscripcion_curso");
                        j.IndexerProperty<int>("IdSuscripcionTipo").HasColumnName("id_suscripcion_tipo");
                        j.IndexerProperty<int>("IdCurso").HasColumnName("id_curso");
                    });
        });

        modelBuilder.Entity<TipoMonedum>(entity =>
        {
            entity.HasKey(e => e.IdTipoMoneda).HasName("PK__tipo_mon__23E2AB815D07CB2C");

            entity.Property(e => e.Codigo).IsFixedLength();
        });

        modelBuilder.Entity<TipoRol>(entity =>
        {
            entity.HasKey(e => e.IdTipoRol).HasName("PK__tipo_rol__60779D18EDE9BE8D");
        });

        modelBuilder.Entity<TipoUsuario>(entity =>
        {
            entity.HasKey(e => e.IdTipoUsuario).HasName("PK__tipo_usu__B17D78C80221684F");
        });

        modelBuilder.Entity<Torneo>(entity =>
        {
            entity.HasKey(e => e.IdTorneo).HasName("PK__torneo__DBB62AF8CB2F357E");

            entity.HasMany(d => d.IdCategoria).WithMany(p => p.IdTorneos)
                .UsingEntity<Dictionary<string, object>>(
                    "TorneoCategorium",
                    r => r.HasOne<Categoria>().WithMany()
                        .HasForeignKey("IdCategoria")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_tc_categoria"),
                    l => l.HasOne<Torneo>().WithMany()
                        .HasForeignKey("IdTorneo")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_tc_torneo"),
                    j =>
                    {
                        j.HasKey("IdTorneo", "IdCategoria").HasName("PK__torneo_c__6763613D13602571");
                        j.ToTable("torneo_categoria");
                        j.IndexerProperty<int>("IdTorneo").HasColumnName("id_torneo");
                        j.IndexerProperty<int>("IdCategoria").HasColumnName("id_categoria");
                    });
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__usuario__4E3E04AD977F21DC");

            entity.Property(e => e.FechaRegistro).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Genero).IsFixedLength();

            entity.HasOne(d => d.IdPaisNavigation).WithMany(p => p.Usuarios).HasConstraintName("FK_usuario_pais");

            entity.HasOne(d => d.IdTipoUsuarioNavigation).WithMany(p => p.Usuarios).HasConstraintName("FK_usuario_tipo_usuario");
        });

        modelBuilder.Entity<Video>(entity =>
        {
            entity.HasKey(e => e.IdRecurso).HasName("PK__video__2B386DE46132DC69");

            entity.Property(e => e.IdRecurso).ValueGeneratedNever();

            entity.HasOne(d => d.IdCapituloNavigation).WithMany(p => p.Videos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_video_capitulo");

            entity.HasOne(d => d.IdRecursoNavigation).WithOne(p => p.Video).HasConstraintName("FK_video_recurso");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
