namespace TransporteApi.Models
{
    public class Pagination
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int Skip => (Page - 1) * PageSize;
        public int Take => PageSize;
    }
}
