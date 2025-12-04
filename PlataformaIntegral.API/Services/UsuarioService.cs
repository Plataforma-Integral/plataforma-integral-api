using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PlataformaIntegral.API.DTOs;
using PlataformaIntegral.API.Models;

namespace PlataformaIntegral.API.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly PlataformaIntegralContext _context;
        private readonly IMapper _mapper;
        private readonly MinioService _minioService;

        public UsuarioService(PlataformaIntegralContext context, IMapper mapper, MinioService minioService)
        {
            _context = context;
            _mapper = mapper;
            _minioService = minioService;
        }

        // =========================================
        // Obtener todos los usuarios
        // =========================================
        public async Task<IEnumerable<UsuarioReadDto>> GetAllAsync()
        {
            var usuarios = await _context.Usuarios
                .Include(u => u.ConfiguracionPrivacidad)
                .ToListAsync();

            var usuariosDto = _mapper.Map<IEnumerable<UsuarioReadDto>>(usuarios);

            foreach (var dto in usuariosDto)
            {
                var usuario = usuarios.First(u => u.IdUsuario == dto.IdUsuario);
                if (!string.IsNullOrEmpty(usuario.ImagenBucketName))
                {
                    dto.ImagenUrl = await _minioService.GetUsuarioImageUrlAsync(
                        usuario.ImagenBucketName,
                        TimeSpan.FromHours(1)
                    );
                }
            }

            return usuariosDto;
        }

        // =========================================
        // Obtener usuario por ID
        // =========================================
        public async Task<UsuarioReadDto?> GetByIdAsync(int id)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.ConfiguracionPrivacidad)
                .FirstOrDefaultAsync(u => u.IdUsuario == id);

            if (usuario == null)
                return null;

            var dto = _mapper.Map<UsuarioReadDto>(usuario);

            if (!string.IsNullOrEmpty(usuario.ImagenBucketName))
            {
                dto.ImagenUrl = await _minioService.GetUsuarioImageUrlAsync(
                    usuario.ImagenBucketName,
                    TimeSpan.FromHours(1)
                );
            }

            return dto;
        }

        // =========================================
        // Crear usuario (incluye imagen opcional)
        // =========================================
        public async Task<UsuarioReadDto> CreateAsync(UsuarioCreateDto dto)
        {
            var usuario = _mapper.Map<Usuario>(dto);

            // Subir imagen si viene en el DTO
            if (dto.ImagenPerfil != null)
            {
                var objectKey = await _minioService.UploadUsuarioImageAsync(dto.ImagenPerfil);
                usuario.ImagenBucketName = objectKey;
            }

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            var usuarioDto = _mapper.Map<UsuarioReadDto>(usuario);

            if (!string.IsNullOrEmpty(usuario.ImagenBucketName))
            {
                usuarioDto.ImagenUrl = await _minioService.GetUsuarioImageUrlAsync(
                    usuario.ImagenBucketName,
                    TimeSpan.FromHours(1)
                );
            }

            return usuarioDto;
        }

        // =========================================
        // Actualizar usuario (incluye imagen)
        // =========================================
        public async Task<bool> UpdateAsync(int id, UsuarioUpdateDto dto)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return false;

            _mapper.Map(dto, usuario);

            if (dto.ImagenPerfil != null)
            {
                if (!string.IsNullOrEmpty(usuario.ImagenBucketName))
                {
                    await _minioService.DeleteUsuarioImageAsync(usuario.ImagenBucketName);
                }

                var objectKey = await _minioService.UploadUsuarioImageAsync(dto.ImagenPerfil);
                usuario.ImagenBucketName = objectKey;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        // =========================================
        // Eliminar usuario (incluye imagen en MinIO)
        // =========================================
        public async Task<bool> DeleteAsync(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return false;

            // Eliminar imagen de MinIO si existe
            if (!string.IsNullOrEmpty(usuario.ImagenBucketName))
            {
                await _minioService.DeleteUsuarioImageAsync(usuario.ImagenBucketName);
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
