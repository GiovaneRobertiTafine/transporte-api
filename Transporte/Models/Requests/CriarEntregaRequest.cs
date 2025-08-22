using System.ComponentModel.DataAnnotations;

namespace TransporteApi.Models.Requests
{
    public class CriarEntregaRequest
    {
        [Required]
        public string Cliente { get; set; } = null!;
        [Required]
        public string Endereco { get; set; } = null!;
        [Required]
        public string Produto { get; set; } = null!;
        [Required]
        public DateTime DataEstimadaEntrega { get; set; }
        public string Observcoes { get; set; } = null!;

        public bool isValid ()
        {
            if (DataEstimadaEntrega.Date <= DateTime.Today) return false;
            return true;
        }
    }
}
