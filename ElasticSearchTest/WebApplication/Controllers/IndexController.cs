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
        public async Task<IActionResult> CreateArticleIndex()
        {
            var result = await _indexService.CreateArticleIndex();

            return Json(result);
        }
    }
}