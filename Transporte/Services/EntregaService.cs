using TransporteApi.Models;
using TransporteApi.Models.Interfaces;
using TransporteApi.Models.Requests;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TransporteApi.Services
{
    public class EntregaService: IEntregaService
    {
        private readonly AppDbContext _context;

        public EntregaService(AppDbContext context)
        {
            this._context = context;
        }
        public async Task CriarEntrega (CriarEntregaRequest request)
        {
            var id = DateTime.Now.Ticks;
            var entrega = new Entrega()
            {
                Id = id,
                Cliente = request.Cliente,
                DataEnvio = DateTime.Now,
                Endereco = request.Endereco,
                DataEstimadaEntrega = request.DataEstimadaEntrega,
                Observacoes = request.Observacoes,
                Produto = request.Produto,
                Posts = new List<HistoricoEntrega>()
                {
                    new HistoricoEntrega()
                    {
                        Id = id,
                        Data = DateTime.Now,
                        Status = StatusEntrega.PEDIDO_CRIADO
                    }
                }
            };

            _context.Entregas.Add(entrega);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Entrega>> ObterEntregas (ObterEntregasRequest request)
        {
            var query = _context.Entregas.AsQueryable();

            if (!string.IsNullOrEmpty(request.ClienteCodigo))
            {
                if (long.TryParse(request.ClienteCodigo, out long codigo))
                {
                    query = query.Where(e => e.Id.ToString().Contains(request.ClienteCodigo));
                } else
                {
                    query = query.Where(e => e.Cliente.Contains(request.ClienteCodigo));
                }
            }

            if (request.Status.HasValue)
            {
                query = query.Where(e =>
                    _context.HistoricoEntregas
                        .Where(h => h.Id == e.Id)
                        .OrderByDescending(h => h.Data)
                        .Select(h => h.Status)
                        .FirstOrDefault() == request.Status.Value);
            }

            return await query
                .Include(e => e.Posts.OrderByDescending(p => p.Data))
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();
        }
    }
}
