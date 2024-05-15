namespace NewsAPIMobilePark.Models
{
    public class Article
    {
        public Source Source { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string URL { get; set; }

        public string URLToImage { get; set; }
        public string PublishedAt { get; set; }
        public string Content { get; set; }

    }
}