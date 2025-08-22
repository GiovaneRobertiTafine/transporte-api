using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;
using System.Text.Json.Serialization;

namespace TransporteApi.Models
{
    public class Entrega
    {
        public string Id { get; set; } = null!;
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
        public string Id { get; set; } = null!;

        [ForeignKey(nameof(Entrega))]
        public string EntregaId { get; set; } = null!;

        public StatusEntrega Status { get; set; }
        public DateTime Data { get; set; }
        public Entrega Entrega { get; init; } = null!;
    }
}
