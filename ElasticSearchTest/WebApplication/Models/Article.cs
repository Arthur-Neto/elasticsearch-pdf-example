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
    }
}
