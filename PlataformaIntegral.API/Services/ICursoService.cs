using PlataformaIntegral.API.DTOs;

namespace PlataformaIntegral.API.Services
{
    public interface ICursoService
    {
        Task<List<CursoCardDto>> ObtenerCursosPopularesAsync(int limite);
        Task<List<CursoCardDto>> ObtenerCursosNuevosAsync(int limite);

        Task<List<CursoCardDto>> BuscarCursosAsync(string? texto, int pagina, int tamañoPagina);
        Task<List<CursoCardDto>> ObtenerCursosPorCategoriaAsync(int categoriaId, int pagina, int tamañoPagina);

        Task<CursoCardDto?> ObtenerCursoCardPorIdAsync(int cursoId); // útil para detalle simple

        // Nuevos:
        Task<List<CursoCardDto>> ObtenerCursosPorUsuarioAsync(int usuarioId); // cursos comprados/inscritos
        Task<CursoPaginaDto?> ObtenerPaginaCursoAsync(int cursoId, int? usuarioId = null, string? rol = null); // usuarioId opcional para ver statuses
        //Task<Stream?> DescargarCursoZipAsync(int cursoId, int usuarioId, CancellationToken ct); // stream zip (null si no autorizado o no pregrabado)
        Task<List<Uri>> ObtenerUrlsDescargaCursoAsync(int cursoId, int usuarioId, int minutesuUrlExpiry, string? rol = null); // alternativa: lista de presigned URLs
        Task<VideoDetalleDto?> ObtenerVideoDetalleAsync(int videoId, int usuarioId, int minutesUrlExpiry, string? rol = null);
    }
}
