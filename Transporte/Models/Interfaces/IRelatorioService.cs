using Microsoft.AspNetCore.Mvc;
using TransporteApi.Models.DTO;

namespace TransporteApi.Models.Interfaces
{
    public interface IRelatorioService
    {
        public Task<List<IndiceStatusDto>> ObterQuatidadePorStatus();
        public Task<int> ObterTotalEntregasAtrasadas();
        public Task<List<QuantidadeEntregaPorDia>> ObterQuantidadeEntregaPorDia(
            DateTime dataInicio,
            DateTime dataFim
        );
    }
}
