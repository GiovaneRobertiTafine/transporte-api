namespace TransporteApi.Models.DTO
{
    public class EntregaDto
    {
        public long Id { get; set; }
        public string Cliente { get; set; } = null!;
        public DateTime DataEnvio { get; set; }
        public string Endereco { get; set; } = null!;
        public string Produto { get; set; } = null!;
        public DateTime DataEstimadaEntrega { get; set; }
        public string Observcoes { get; set; } = null!;
        public StatusEntrega Status { get; set; }

        public ICollection<HistoricoEntrega> Historico { get; set; } = new List<HistoricoEntrega>();

        public EntregaDto (Entrega entrega) 
        {
            Id = entrega.Id;
            Cliente = entrega.Cliente;
            Produto = entrega.Produto;
            Endereco = entrega.Endereco;
            DataEnvio = entrega.DataEnvio;
            DataEstimadaEntrega = entrega.DataEstimadaEntrega;
            Observcoes = entrega.Observcoes;
            Status = entrega.Posts.FirstOrDefault()!.Status;
            Historico = entrega.Posts.Skip(1).ToList();
        }
    }
}
