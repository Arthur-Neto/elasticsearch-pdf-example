using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApplication.Services;

namespace WebApplication.Controllers
{
    [Route("api/index")]
    public class IndexController : Controller
    {
        private readonly IndexService _indexService;

        public IndexController(IndexService indexService)
        {
            _indexService = indexService;
        }

        [HttpGet]
        [Route("create")]
        public async Task<IActionResult> CreateArticleIndex()
        {
            var result = await _indexService.CreateArticleIndex();

            return Ok(result);
        }

        [HttpGet]
        [Route("mapped")]
        public async Task<IActionResult> GetIndexes()
        {
            var result = await _indexService.GetMappedIndexes();

            return Ok(result);
        }
    }
}