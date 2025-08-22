using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;

namespace TransporteApi.Models
{
    public class Entrega
    {
        public long Id { get; set; }
        public string Cliente { get; set; } = null!;
        public DateTime DataEnvio { get; set; }
        public string Endereco { get; set; } = null!;
        public string Produto { get; set; } = null!;
        public DateTime DataEstimadaEntrega { get; set; }
        public string? Observacoes { get; set; } = null!;

        public ICollection<HistoricoEntrega> Posts { get; set; } = new List<HistoricoEntrega>();
    }

    public class HistoricoEntrega
    {
        public long Id { get; set; }

        public StatusEntrega Status { get; set; }
        public DateTime Data { get; set; }
    }
}
