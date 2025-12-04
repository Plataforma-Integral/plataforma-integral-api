using AutoMapper;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PlataformaIntegral.API.DTOs.Auth;
using PlataformaIntegral.API.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace PlataformaIntegral.API.Services.Auth
{
    public class AuthService : Services.Auth.IAuthService
    {
        private readonly PlataformaIntegralContext _context;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly MinioService _minioService;

        public AuthService(PlataformaIntegralContext context, IConfiguration config, IMapper mapper, MinioService minioService)
        {
            _context = context;
            _config = config;
            _mapper = mapper;
            _minioService = minioService;
        }

        public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
        {
            var credencial = await _context.Credenciales
                .Include(c => c.Usuario)
                .ThenInclude(u => u.TipoUsuario)
                .FirstOrDefaultAsync(c => c.Email == dto.Email);

            if (credencial == null)
            {
                return null;
            }

            if (credencial.Contrasena == null ||
                !BCrypt.Net.BCrypt.Verify(dto.Contrasena, credencial.Contrasena))
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Credenciales inválidas"
                };
            }

            // Generar token y respuesta
            var response = GenerateToken(credencial.Usuario);

            // Obtener imagen del usuario si existe
            if (!string.IsNullOrEmpty(credencial.Usuario.ImagenBucketName))
            {
                response.ImagenUrl = await _minioService.GetUsuarioImageUrlAsync(
                    credencial.Usuario.ImagenBucketName,
                    TimeSpan.FromHours(1) // expira en 1 hora
                );
            }

            return response;
        }


        public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
        {
            // Verificar si ya existe el email
            if (await _context.Credenciales.AnyAsync(c => c.Email == dto.Email))
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "El email ya está registrado"
                };

            var usuario = new Usuario
            {
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                FechaRegistro = DateTime.UtcNow,
                IdTipoUsuario = dto.IdTipoUsuario > 0 || dto.IdTipoUsuario < 4  ? dto.IdTipoUsuario : 1 // 1 = Estudiante por defecto
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            var ConfiguracionPrivacidad = new ConfiguracionPrivacidad
            {
                IdUsuario = usuario.IdUsuario,
                MostrarEmail = false,
                MostrarTelefono = false,
                MostrarFechaNacimiento = false
            };
            _context.ConfiguracionesPrivacidad.Add(ConfiguracionPrivacidad);

            var credencial = new Credencial
            {
                Email = dto.Email,
                Contrasena = BCrypt.Net.BCrypt.HashPassword(dto.Contrasena),
                Proveedor = "Local",
                FechaCreacion = DateTime.UtcNow,
                IdUsuario = usuario.IdUsuario
            };

            _context.Credenciales.Add(credencial);

            // 4️ Crear rol específico
            switch (usuario.IdTipoUsuario)
            {
                case 1: // Estudiante
                    _context.Estudiantes.Add(new Estudiante
                    {
                        IdUsuario = usuario.IdUsuario,
                        Educacion = dto.NivelEducativo,
                        Puntos = 0
                    });
                    break;

                case 2: // Profesor
                    _context.Profesores.Add(new Profesor
                    {
                        IdUsuario = usuario.IdUsuario,
                        Disponibilidad = dto.Disponibilidad,
                        EstadoVerificacion = false
                    });
                    break;

                case 3: // Administrador
                    _context.Administradores.Add(new Administrador
                    {
                        IdUsuario = usuario.IdUsuario,
                        Rol = dto.Rol ?? "Administrador"
                    });
                    break;

                default:
                    _context.Estudiantes.Add(new Estudiante { IdUsuario = usuario.IdUsuario });
                    break;
            }
            await _context.SaveChangesAsync();

            // Cargar el usuario con las relaciones necesarias para el token
            var usuarioCompleto = await _context.Usuarios
                .Include(u => u.TipoUsuario)
                .Include(u => u.Credenciales)
                .FirstOrDefaultAsync(u => u.IdUsuario == usuario.IdUsuario);

            if (usuarioCompleto == null)
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Error al crear la cuenta"
                };

            return GenerateToken(usuarioCompleto);
        }

        private AuthResponseDto GenerateToken(Usuario usuario)
        {
            var role = usuario.TipoUsuario?.Nombre ?? "Estudiante";

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.IdUsuario.ToString()),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Email,
                          usuario.Credenciales.First().Email)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"]!)
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddHours(3);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: creds
            );

            return new AuthResponseDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration,
                IdUsuario = usuario.IdUsuario,
                Email = usuario.Credenciales.First().Email,
                Rol = role,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido
            };
        }

        public string GenerateJwtToken(string email, int idUsuario, string role)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, idUsuario.ToString()),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Email, email)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"]!)
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddHours(3);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
