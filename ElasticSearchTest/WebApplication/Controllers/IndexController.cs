using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApplication.Elastic;

namespace WebApplication.Controllers
{
    [Route("/api/[controller]")]
    public class IndexController : Controller
    {
        private readonly DataIndexer _indexer;

        public IndexController(DataIndexer indexer)
        {
            _indexer = indexer;
        }

        [HttpGet("file")]
        public async Task<IActionResult> IndexDataFromFile([FromQuery]string fileName, string index, bool deleteIndexIfExists)
        {
            var response = await _indexer.IndexDocumentsFromFile(fileName, deleteIndexIfExists, index);
            return Ok(response);
        }
    }
}
