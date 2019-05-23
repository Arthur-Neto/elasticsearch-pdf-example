using Nest;
using System;
using System.IO;
using System.Threading.Tasks;
using WebApplication.Elastic;
using WebApplication.Models;

namespace WebApplication.Services
{
    public class ArticleService
    {
        private readonly ElasticClientProvider _esProvider;

        public ArticleService(ElasticClientProvider esProvider)
        {
            _esProvider = esProvider;
        }

        public async Task<IIndexResponse> IndexArticleByPath(string path)
        {
            var base64File = Convert.ToBase64String(File.ReadAllBytes(path));
            var indexReturn = await _esProvider.Client.IndexAsync(new Article
            {
                Id = 1,
                Path = path,
                Content = base64File
            }, i => i
                .Index("article_index")
                .Pipeline("articles_pipeline")
                .Timeout("5m")
            );

            return indexReturn;
        }

        public async Task<ISearchResponse<Article>> SearchArticles(string query)
        {
            var searchResponse = await _esProvider.Client.SearchAsync<Article>(s => s
                .Query(q => q
                    .Match(m => m
                        .Field(a => a.Attachment.Content)
                        .Query(query)
                    )
                )
            );

            return searchResponse;
        }
    }
}
