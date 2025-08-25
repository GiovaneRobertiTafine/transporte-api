using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransporteApi.Models;
using TransporteApi.Models.DTO;
using TransporteApi.Models.Interfaces;

namespace TransporteApi.Services
{
    public class RelatorioService: IRelatorioService
    {

        private readonly AppDbContext _context;

        public RelatorioService(AppDbContext context)
        {
            this._context = context;
        }

        public async Task<List<IndiceStatusDto>> ObterQuatidadePorStatus()
        {
            var res = await _context.Entregas
                                    .Select(e => e.Posts.OrderByDescending(p => p.Data)
                                                        .FirstOrDefault()!.Status)
                                    .GroupBy(status => status)
                                    .Select(g => new IndiceStatusDto
                                    {
                                        Status = g.Key,
                                        Quantidade = g.Count()
                                    })
                                    .ToListAsync();

            List<IndiceStatusDto> lista = [.. res];

            var todosStatus = Enum.GetValues<StatusEntrega>();
            foreach (var status in todosStatus)
            {
                if (!lista.Any(s => s.Status == status))
                {
                    lista.Add(new IndiceStatusDto
                    {
                        Status = status,
                        Quantidade = 0
                    });
                }
            }

            return lista.OrderBy((s) => s.Status).ToList();
        }

        public async Task<int> ObterTotalEntregasAtrasadas()
        {
            var hoje = DateTime.Now;
            var totalAtrasadas = await _context.Entregas
                .Where(e => e.DataEstimadaEntrega < hoje)
                .Where(e => e.Posts.OrderByDescending(p => p.Data).FirstOrDefault()!.Status != StatusEntrega.ENTREGUE)
                .CountAsync();
            return totalAtrasadas;
        }

        public async Task<List<QuantidadeEntregaPorDia>> ObterQuantidadeEntregaPorDia(
            DateTime dataInicio,
            DateTime dataFim
        )
        {
            if (dataInicio > dataFim)
            {
                throw new ArgumentException("Data de início deve ser menor ou igual à data de fim.");
            }
            var entregasPorDia = await _context.Entregas
                .Where(e => e.DataEstimadaEntrega.Date >= dataInicio.Date && e.DataEstimadaEntrega.Date <= dataFim.Date)
                .GroupBy(e => e.DataEstimadaEntrega.Date)
                .Select(g => new QuantidadeEntregaPorDia
                {
                    Dia = g.Key,
                    Quantidade = g.Count()
                })
                .ToListAsync();

            var totalDias = (dataFim.Date - dataInicio.Date).Days + 1;
            var result = new List<QuantidadeEntregaPorDia>();
            for (int i = 0; i < totalDias; i++)
            {
                var diaAtual = dataInicio.Date.AddDays(i);
                var entregaDoDia = entregasPorDia.FirstOrDefault(e => e.Dia == diaAtual);
                if (entregaDoDia != null)
                {
                    result.Add(entregaDoDia);
                }
                else
                {
                    result.Add(new QuantidadeEntregaPorDia
                    {
                        Dia = diaAtual,
                        Quantidade = 0
                    });
                }
            }
            return result.OrderBy(e => e.Dia).ToList();
        }
    }
}
