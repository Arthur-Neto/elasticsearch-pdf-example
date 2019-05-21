using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Models;

namespace WebApplication.Elastic
{
    public class SearchService
    {
        private readonly ElasticClient client;

        public SearchService(ElasticClientProvider clientProvider)
        {
            this.client = clientProvider.Client;
        }

        public async Task<SearchResult<Document>> Search(string query, int page, int pageSize)
        {
            var response = await this.client.SearchAsync<Document>(searchDescriptor => searchDescriptor
                    .Query(queryContainerDescriptor => queryContainerDescriptor
                        .Bool(queryDescriptor => queryDescriptor
                            .Must(queryStringQuery => queryStringQuery
                                .QueryString(queryString => queryString
                                    .Query(query)))))
                                        .From((page - 1) * pageSize)
                                        .Size(pageSize));

            return new SearchResult<Document>
            {
                Total = response.Total,
                ElapsedMilliseconds = response.Took,
                Page = page,
                PageSize = pageSize,
                Results = response.Documents
            };
        }

        public async Task<List<AutocompleteResult>> Autocomplete(string query)
        {
            var response = await this.client.SearchAsync<Document>(sr => sr
                .Suggest(scd => scd
                    .Completion("document-title-completion", cs => cs
                        .Prefix(query)
                        .Fuzzy(fsd => fsd
                            .Fuzziness(Fuzziness.Auto))
                        .Field(r => r.Titulo))));

            var suggestions = this.ExtractAutocompleteSuggestions(response);

            return suggestions;
        }

        private List<AutocompleteResult> ExtractAutocompleteSuggestions(ISearchResponse<Document> response)
        {
            var results = new List<AutocompleteResult>();

            var suggestions = response.Suggest["document-title-completion"].Select(s => s.Options);

            suggestions.ToList().ForEach(s =>
            {
                results.AddRange(s.Select(opt => new AutocompleteResult
                {
                    Id = opt.Source.Id,
                    Titulo = opt.Source.Titulo
                }));
            });

            return results;
        }
    }
}
