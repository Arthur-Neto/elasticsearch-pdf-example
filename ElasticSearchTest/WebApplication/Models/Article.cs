using Nest;

namespace WebApplication.Models
{
    public class Article
    {
        public string Path { get; set; }
        public string Content { get; set; }
        public Attachment Attachment { get; set; }
    }
}
