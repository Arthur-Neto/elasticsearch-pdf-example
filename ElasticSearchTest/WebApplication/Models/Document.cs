using Nest;
using System.Collections.Generic;

namespace WebApplication.Models
{
    // Specify the type name
    [ElasticsearchType(Name = "document")]
    public class Document
    {
        public string Id { get; set; }
        [Completion]
        public string Titulo { get; set; }
        // Specify the * as a text field to enable full-text search
        [Text]
        public List<string> Autores { get; set; }
        [Text]
        public string Curso { get; set; }
        public string Orientador { get; set; }
    }
}
