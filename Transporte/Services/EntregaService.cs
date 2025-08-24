using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using TransporteApi.Models;
using TransporteApi.Models.DTO;
using TransporteApi.Models.Interfaces;
using TransporteApi.Models.Requests;
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
            var id = DateTime.Now.Ticks.ToString();
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
                        Id = Guid.NewGuid().ToString(),
                        EntregaId = id,
                        Data = DateTime.Now,
                        Status = StatusEntrega.PEDIDO_CRIADO
                    }
                }
            };

            _context.Entregas.Add(entrega);
            await _context.SaveChangesAsync();
        }

        public async Task<PaginationResult<Entrega>> ObterEntregas (ObterEntregasRequest request)
        {
            var query = _context.Entregas.AsQueryable();

            if (!string.IsNullOrEmpty(request.ClienteCodigo))
            {
                if (decimal.TryParse(request.ClienteCodigo, out decimal codigo))
                {
                    query = query.Where(e => e.Id.Contains(request.ClienteCodigo));
                } else
                {
                    query = query.Where(e => e.Cliente.Contains(request.ClienteCodigo));
                }
            }

            if (request.Status.HasValue)
            {
                query = query.Where(e =>
                    _context.HistoricoEntregas
                        .Where(h => h.EntregaId == e.Id)
                        .OrderByDescending(h => h.Data)
                        .Select(h => h.Status)
                        .FirstOrDefault() == request.Status.Value);
            }

            var itens = await query
                .AsNoTracking()
                .Include(e => e.Posts.OrderByDescending(p => p.Data))
                .OrderBy(e => e.DataEnvio)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            return new PaginationResult<Entrega>(itens, await query.CountAsync());
        }

        public async Task<Entrega> ObterEntregaPorId(string id)
        {
            var entrega = await _context.Entregas
            .AsNoTracking()
            .Where(e => e.Id == id)
            .Include(e => e.Posts)
            .FirstOrDefaultAsync();

            if (entrega == null)
            {
                throw new ArgumentException($"Entrega com ID '{id}' não encontrada.");
            }

            return entrega;
        }

        public async Task<Entrega> AlterarStatusEntrega(string id, StatusEntrega status)
        {
            var entrega = await _context.Entregas
            .Include(e => e.Posts)
            .FirstOrDefaultAsync(e => e.Id == id);

            if (entrega == null)
            {
                throw new ArgumentException($"Entrega com ID '{id}' não encontrada.");
            }
               

            var ultimoStatus = entrega.Posts.OrderBy(h => h.Data).FirstOrDefault()!.Status;
            if (ultimoStatus == StatusEntrega.CANCELADA)
            {
                throw new ArgumentException($"Entrega com status cancelada.");
            }

            var novoHistorico = new HistoricoEntrega
            {
                Id = Guid.NewGuid().ToString(),
                EntregaId = id,
                Status = status,
                Data = DateTime.Now
            };

            entrega.Posts.Add(novoHistorico);

            await _context.SaveChangesAsync();

            return entrega;
        }
    }
}
