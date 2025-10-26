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

        public AuthService(PlataformaIntegralContext context, IConfiguration config, IMapper mapper)
        {
            _context = context;
            _config = config;
            _mapper = mapper;
        }

        public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
        {
            var credencial = await _context.Credenciales
                .Include(c => c.Usuario)
                .ThenInclude(u => u.TipoUsuario)
                .FirstOrDefaultAsync(c => c.Email == dto.Email);

            if (credencial == null || credencial.Contrasena == null ||
                !BCrypt.Net.BCrypt.Verify(dto.Contrasena, credencial.Contrasena))
                return null;

            return GenerateToken(credencial.Usuario);
        }

        // Reemplazar el método RegisterAsync existente para que coincida con la interfaz
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
                IdTipoUsuario = dto.IdTipoUsuario ?? 3 // 3 = Estudiante por defecto
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
                case 1:
                    _context.Estudiantes.Add(new Estudiante { IdUsuario = usuario.IdUsuario });
                    break;
                case 2:
                    _context.Profesores.Add(new Profesor { IdUsuario = usuario.IdUsuario });
                    break;
                case 3:
                    _context.Administradores.Add(new Administrador { IdUsuario = usuario.IdUsuario });
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

            if (credencial == null ||
                credencial.Contrasena == null ||
                !BCrypt.Net.BCrypt.Verify(dto.Contrasena, credencial.Contrasena))
            {
                return new AuthResponseDto
                {
                    Token = null,
                    Success = false,
                    Message = "Credenciales incorrectas"
                };
            }


            return GenerateToken(usuarioCompleto);
        }

        // Implementar el método GenerateJwtToken requerido por la interfaz
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
    }
}
