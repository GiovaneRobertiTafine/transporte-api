namespace TransporteApi.Models.DTO
{
    public class EntregaDto
    {
        public string Id { get; set; }
        public string Cliente { get; set; } = null!;
        public DateTime DataEnvio { get; set; }
        public string Endereco { get; set; } = null!;
        public string Produto { get; set; } = null!;
        public DateTime DataEstimadaEntrega { get; set; }
        public string Observacoes { get; set; } = null!;
        public StatusEntrega Status { get; set; }

        public ICollection<HistoricoEntregaDto> Historico { get; set; } = new List<HistoricoEntregaDto>();

        public EntregaDto(Entrega entrega) 
        {
            Id = entrega.Id;
            Cliente = entrega.Cliente;
            Produto = entrega.Produto;
            Endereco = entrega.Endereco;
            DataEnvio = entrega.DataEnvio;
            DataEstimadaEntrega = entrega.DataEstimadaEntrega;
            Observacoes = entrega.Observacoes ?? "";
            Status = entrega.Posts.OrderByDescending(h => h.Data).FirstOrDefault()!.Status;
            Historico = entrega.Posts
                .OrderByDescending(h => h.Data)
                .Skip(1)
                .Select(h => new HistoricoEntregaDto
                {
                    Id = h.Id,
                    Status = h.Status,
                    Data = h.Data
                })
                .ToList();
        }

        public EntregaDto (string id, string cliente, DateTime dataEnvio, ICollection<HistoricoEntrega> posts) 
        {
            Id = id;
            Cliente = cliente;
            DataEnvio = dataEnvio;
            Status = posts.OrderByDescending(h => h.Data).FirstOrDefault()!.Status;
            Produto = "";
            Endereco = "";
            DataEstimadaEntrega = DateTime.MinValue;
            Observacoes = "";
        }
    }
}
