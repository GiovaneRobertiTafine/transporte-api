namespace TransporteApi.Models.Requests
{
    public class ObterEntregasRequest
    {
        public StatusEntrega? Status { get; set; }
        public string? ClienteCodigo { get; set; }
    }
}
