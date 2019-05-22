using Nest;
using System;

namespace WebApplication.Models
{
    public class Article
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public string Content { get; set; }
        public Attachment Attachment { get; set; }

        public string Title { get; set; }
        public string Author { get; set; }
        public string Keywords { get; set; }
        public DateTime Date { get; set; }
    }
}
