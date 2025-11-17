using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlataformaIntegral.API.DTOs;
using PlataformaIntegral.API.Enums;
using PlataformaIntegral.API.Models;
using PlataformaIntegral.API.Services;

public class CrearCursoService : ICrearCursoService
{
    private readonly PlataformaIntegralContext _context;
    private readonly IMapper _mapper;
    private readonly MinioService _storage;

    public CrearCursoService(
        PlataformaIntegralContext context,
        IMapper mapper,
        MinioService storage)
    {
        _context = context;
        _mapper = mapper;
        _storage = storage;
    }

    // ========================================
    // 1. CREAR CURSO PREGRABADO
    // ========================================
    public async Task<int> CrearCursoPregrabadoAsync([FromForm] CursoPregrabadoDto dto)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var producto = new Producto
            {
                Nombre = dto.Titulo,
                Descripcion = dto.Descripcion,
                Precio = dto.Precio,
                FechaCreacion = DateTime.UtcNow
            };

            var curso = new Curso
            {
                Estado = EstadoCursoEnum.Borrador,
                Producto = producto
            };

            var cursoPregrabado = new CursoPregrabado
            {
                Curso = curso,
                PrecioPuntos = dto.PrecioPuntos
            };

            _context.CursoPregrabados.Add(cursoPregrabado);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return cursoPregrabado.IdCurso;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    // ========================================
    // 2. AGREGAR CAPÍTULO
    // ========================================
    public async Task<int> AgregarCapituloAsync(int cursoPregrabadoId, string nombreCapitulo)
    {
        var numero = await _context.Capitulos
            .CountAsync(c => c.IdCursoPregrabado == cursoPregrabadoId) + 1;

        var capitulo = new Capitulo
        {
            IdCursoPregrabado = cursoPregrabadoId,
            Nombre = nombreCapitulo,
            NumeroOrden = numero
        };

        _context.Capitulos.Add(capitulo);
        await _context.SaveChangesAsync();

        return capitulo.IdCapitulo;
    }

    // ========================================
    // 3. SUBIR VIDEO
    // ========================================
    public async Task<int> SubirVideoAsync(int capituloId, [FromForm] VideoDto dto)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            // 1) Subir archivo a MinIO → devuelve objectKey
            string objectKey = null;

            if (dto.Archivo != null)
            {
                objectKey = await _storage.UploadVideoAsync(dto.Archivo);
            }

            // 2) Crear entidad Recurso
            var recurso = new Recurso
            {
                Nombre = dto.Nombre,
                Url = objectKey,        // guardamos la KEY, no una URL completa
            };

            _context.Recursos.Add(recurso);
            await _context.SaveChangesAsync();

            // 3) Obtener número de orden del video
            var numeroOrden = await _context.Videos
                .CountAsync(v => v.IdCapitulo == capituloId) + 1;

            // 4) Crear entidad Video
            var video = new Video
            {
                IdCapitulo = capituloId,
                IdRecurso = recurso.IdRecurso,
                NumeroOrden = numeroOrden,
            };

            _context.Videos.Add(video);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            return recurso.IdRecurso;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    // ========================================
    // 4. PUBLICAR CURSO
    // ========================================
    public async Task PublicarCursoAsync(int cursoId)
    {
        var curso = await _context.Cursos.FindAsync(cursoId);

        if (curso == null)
            throw new Exception($"Curso {cursoId} no encontrado.");

        curso.Estado = EstadoCursoEnum.Publicado;

        await _context.SaveChangesAsync();
    }
}
