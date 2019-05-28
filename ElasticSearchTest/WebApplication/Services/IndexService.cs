using Nest;
using System.Threading.Tasks;
using WebApplication.Elastic;
using WebApplication.Models;

namespace WebApplication.Services
{
    public class IndexService
    {
        private readonly ElasticClientProvider _esProvider;

        public IndexService(ElasticClientProvider esProvider)
        {
            _esProvider = esProvider;
        }

        public async Task<ICreateIndexResponse> CreateArticleIndex()
        {
            await _esProvider.Client.DeleteIndexAsync("article_index");

            var indexResponse = await _esProvider.Client.CreateIndexAsync("article_index", c => c
                .Settings(s => s
                    .Analysis(a => a
                        .Analyzers(ad => ad
                            .Custom("windows_path_hierarchy_analyzer", ca => ca
                                .Tokenizer("windows_path_hierarchy_tokenizer")
                            )
                        )
                        .Tokenizers(t => t
                            .PathHierarchy("windows_path_hierarchy_tokenizer", ph => ph
                                .Delimiter('\\')
                            )
                        )
                    )
                )
                .Mappings(m => m
                    .Map<Article>(mp => mp
                        .AutoMap()
                        .AllField(all => all
                            .Enabled(false)
                        )
                        .Properties(ps => ps
                            .Text(s => s
                                .Name(n => n.Path)
                                .Analyzer("windows_path_hierarchy_analyzer")
                            )
                            .Object<Attachment>(a => a
                                .Name(n => n.Attachment)
                                .AutoMap()
                             )
                        )
                    )
                )
            );


            await _esProvider.Client.PutPipelineAsync("articles_pipeline", p => p
                .Processors(pr => pr
                    .Attachment<Article>(a => a
                      .Field(f => f.Content)
                      .TargetField(f => f.Attachment)
                    )
                    .Remove<Article>(r => r
                        .Field(f => f.Content)
                    )
                )
            );

            return indexResponse;
        }

        public async Task<IGetMappingResponse> GetMappedIndexes()
        {
            var mappingResponse = await _esProvider.Client.GetMappingAsync<Article>();

            return mappingResponse;
        }
    }
}
