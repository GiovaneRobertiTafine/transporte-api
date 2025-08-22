using TransporteApi.Models.Requests;

namespace TransporteApi.Models.Interfaces
{
    public interface IEntregaService
    {
        public Task CriarEntrega(CriarEntregaRequest request);
        public Task<PaginationResult<Entrega>> ObterEntregas(ObterEntregasRequest request);
    }
}
