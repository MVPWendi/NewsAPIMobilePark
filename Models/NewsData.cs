namespace NewsAPIMobilePark.Models
{
    public class NewsData
    {
        public string? Status { get; set; }
        public int TotalResults { get; set; }

        public IEnumerable<Article>? Articles { get; set; }
    }
}
