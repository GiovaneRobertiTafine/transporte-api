namespace TransporteApi.Models.DTO
{
    public class ResponseHelperPaginado<T>(T resultado, int tamanhoColecao)
    {
        public T Resultado { get; set; } = resultado;
        public int TamanhoColecao { get; set; } = tamanhoColecao;
    }
}
