using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Nest;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Models;

namespace WebApplication.Elastic
{
    public class DataIndexer
    {
        private readonly ElasticClient client;
        private readonly string contentRootPath;
        private readonly string defaultIndex;

        public DataIndexer(ElasticClientProvider clientProvider, IHostingEnvironment env, IOptions<ElasticConnectionSettings> settings)
        {
            this.client = clientProvider.Client; // Get the ElasticClient
            this.contentRootPath = Path.Combine(env.ContentRootPath, "data"); // Where we'll be looking for the file to read and index
            this.defaultIndex = settings.Value.DefaultIndex; // The default index
        }

        public async Task<bool> IndexDocumentsFromFile(string fileName, bool deleteIndexIfExists, string index = null)
        {
            if (index == null)
            {
                index = this.defaultIndex;
            }
            else
            {
                index = index.ToLower(); // Sempre deve ser minusculo
            }

            using (var fs = new FileStream(Path.Combine(contentRootPath, fileName), FileMode.Open))
            {
                using (var reader = new StreamReader(fs))
                {
                    // Won't be efficient with large files, but better for brevity
                    string rawJsonCollection = await reader.ReadToEndAsync();

                    var mappedCollection = JsonConvert.DeserializeObject<Document[]>(rawJsonCollection, new JsonSerializerSettings
                    {
                        Error = HandleDeserializationError
                    });

                    // If the user specified to drop the index prior to indexing the documents. Useful when you want to "hard reset" things
                    if (this.client.IndexExists(index).Exists && deleteIndexIfExists)
                    {
                        await this.client.DeleteIndexAsync(index);
                    }

                    if (!this.client.IndexExists(index).Exists)
                    {
                        // Automap means that it will be creating the mapping according to the model's attributes
                        var indexDescriptor = new CreateIndexDescriptor(index)
                                        .Mappings(mappings => mappings
                                            .Map<Document>(m => m.AutoMap()));

                        await this.client.CreateIndexAsync(index, i => indexDescriptor);
                    }

                    // Max out the result window so you can have pagination for >100 pages
                    this.client.UpdateIndexSettings(index, ixs => ixs
                         .IndexSettings(s => s
                             .Setting("max_result_window", int.MaxValue)));

                    // Then index the documents
                    var batchSize = 10000; // magic :O
                    var totalBatches = (int)Math.Ceiling((double)mappedCollection.Length / batchSize);

                    for (int i = 0; i < totalBatches; i++)
                    {
                        var response = await this.client.IndexManyAsync(mappedCollection.Skip(i * batchSize).Take(batchSize));
                        if (!response.IsValid)
                        {
                            return false;
                        }
                    }

                    return true;
                }
            }
        }

        // https://stackoverflow.com/questions/26107656/ignore-parsing-errors-during-json-net-data-parsing
        private void HandleDeserializationError(object sender, Newtonsoft.Json.Serialization.ErrorEventArgs errorArgs)
        {
            var currentError = errorArgs.ErrorContext.Error.Message;
            errorArgs.ErrorContext.Handled = true;
        }
    }
}
