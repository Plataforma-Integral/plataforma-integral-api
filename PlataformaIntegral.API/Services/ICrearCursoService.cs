using PlataformaIntegral.API.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlataformaIntegral.API.Services
{
    public interface ICrearCursoService
    {
        // 1. Crear curso pregrabado en borrador
        Task<int> CrearCursoPregrabadoAsync(CursoPregrabadoDto dto);

        // 2. Agregar un capítulo a un curso
        Task<int> AgregarCapituloAsync(int cursoId, string nombreCapitulo);

        // 3. Subir video al capítulo
        Task<int> SubirVideoAsync(int capituloId, VideoDto dto);

        // 4. Publicar el curso
        Task PublicarCursoAsync(int cursoId);
    }
}
