using PlataformaIntegral.API.DTOs;
using System.Threading.Tasks;

namespace PlataformaIntegral.API.Services
{
    public interface ICrearCursoService
    {
        // ---------------------------------------------------------
        // 1. CREACIÓN
        // ---------------------------------------------------------

        /// Crear curso pregrabado en estado borrador
        Task<int> CrearCursoPregrabadoAsync(CursoPregrabadoDto dto);

        /// Agregar capítulo a un curso
        Task<int> AgregarCapituloAsync(int cursoId, string nombreCapitulo);

        /// Subir un video nuevo a un capítulo
        Task<int> SubirVideoAsync(int capituloId, VideoDto dto);

        /// Publicar curso
        Task PublicarCursoAsync(int cursoId);


        // ---------------------------------------------------------
        // 2. EDICIÓN (UPDATE)
        // ---------------------------------------------------------

        /// Editar datos generales del curso (título, descripción, portada, precio…)
        Task ModificarCursoPregrabadoAsync(int cursoId, CursoPregrabadoDto dto);

        /// Editar un capítulo (solo nombre por ahora)
        Task ModificarCapituloAsync(int capituloId, string nuevoNombre);

        /// Editar datos de un video (nombre, descripción) + opcional reemplazar archivo
        Task ModificarVideoAsync(int videoId, VideoDto dto);


        // ---------------------------------------------------------
        // 3. ELIMINACIÓN
        // ---------------------------------------------------------

        // Eliminar un curso completo (y sus capítulos, videos + cuestionarios)
        Task EliminarCursoAsync(int cursoId);

        /// Eliminar un capítulo completo (y sus videos + cuestionarios)
        Task EliminarCapituloAsync(int capituloId);

        /// Eliminar un video (y archivo del almacenamiento)
        Task EliminarVideoAsync(int videoId);
    }
}
