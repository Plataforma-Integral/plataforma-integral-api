using Microsoft.EntityFrameworkCore;
using PlataformaIntegral.API.DTOs;
using PlataformaIntegral.API.Models;
using System;

namespace PlataformaIntegral.API.Services
{
    public class CatalogoService : ICatalogoService
    {
        private readonly PlataformaIntegralContext _context;

        public CatalogoService(PlataformaIntegralContext context)
        {
            _context = context;
        }

        public async Task<List<CatalogoDto>> ObtenerPaisesAsync()
        {
            return await _context.Paises
                .Select(p => new CatalogoDto { Id = p.IdPais, Nombre = p.Nombre, Codigo = p.CodigoIso })
                .ToListAsync();
        }

        public async Task<List<CatalogoDto>> ObtenerTiposUsuarioAsync()
        {
            return await _context.TipoUsuarios
                .Select(t => new CatalogoDto { Id = t.IdTipoUsuario, Nombre = t.Nombre })
                .ToListAsync();
        }

        public async Task<List<CatalogoDto>> ObtenerNivelesMedallaAsync()
        {
            return await _context.NivelMedallas
                .Select(n => new CatalogoDto { Id = n.IdNivelMedalla, Nombre = n.Nombre })
                .ToListAsync();
        }

        public async Task<List<CatalogoDto>> ObtenerModalidadesSincronicoAsync()
        {
            return await _context.ModalidadSincronicos
                .Select(m => new CatalogoDto { Id = m.IdModalidad, Nombre = m.Nombre })
                .ToListAsync();
        }

        public async Task<List<CatalogoDto>> ObtenerTiposRolAsync()
        {
            return await _context.TipoRoles
                .Select(r => new CatalogoDto { Id = r.IdTipoRol, Nombre = r.Nombre })
                .ToListAsync();
        }

        public async Task<List<CatalogoDto>> ObtenerEstadosSuscripcionAsync()
        {
            return await _context.EstadoSuscripcions
                .Select(e => new CatalogoDto { Id = e.IdEstadoSuscripcion, Nombre = e.Nombre })
                .ToListAsync();
        }

        public async Task<List<CatalogoDto>> ObtenerEstadosPagoAsync()
        {
            return await _context.EstadoPagos
                .Select(e => new CatalogoDto { Id = e.IdEstadoPago, Nombre = e.Nombre })
                .ToListAsync();
        }

        public async Task<List<CatalogoDto>> ObtenerMetodosPagoAsync()
        {
            return await _context.MetodoPagos
                .Select(m => new CatalogoDto { Id = m.IdMetodoPago, Nombre = m.Nombre })
                .ToListAsync();
        }

        public async Task<List<CatalogoDto>> ObtenerTiposMonedaAsync()
        {
            return await _context.TipoMoneda
                .Select(m => new CatalogoDto { Id = m.IdTipoMoneda, Nombre = m.Nombre, Codigo = m.Codigo })
                .ToListAsync();
        }
    }
}
