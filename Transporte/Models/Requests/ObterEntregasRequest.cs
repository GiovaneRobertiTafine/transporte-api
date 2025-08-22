namespace TransporteApi.Models.Requests
{
    public class ObterEntregasRequest: Pagination
    {
        public StatusEntrega? Status { get; set; }
        public string? ClienteCodigo { get; set; }
    }
}
