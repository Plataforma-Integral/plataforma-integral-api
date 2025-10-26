using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlataformaIntegral.API.DTOs;
using PlataformaIntegral.API.Models;

namespace PlataformaIntegral.API.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly PlataformaIntegralContext _context;
        private readonly IMapper _mapper;

        public UsuarioService(PlataformaIntegralContext context, IMapper mapper) 
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<IEnumerable<UsuarioReadDto>> GetAllAsync()
        {
            var usuarios = await _context.Usuarios.ToListAsync();
            return _mapper.Map<IEnumerable<UsuarioReadDto>>(usuarios);
        }

        public async Task<UsuarioReadDto?> GetByIdAsync(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return null;

            return _mapper.Map<UsuarioReadDto>(usuario);
        }

        public async Task<bool> UpdateAsync(int id, UsuarioCreateDto dto)
        {
            var usuario = _context.Usuarios.Find(id);
            if (usuario == null)
                return false;

            _mapper.Map(dto, usuario);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
