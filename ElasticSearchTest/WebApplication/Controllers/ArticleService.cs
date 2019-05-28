using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApplication.Services;

namespace WebApplication.Controllers
{
    [Route("api/article")]
    public class ArticleController : Controller
    {
        private readonly ArticleService _articleService;

        public ArticleController(ArticleService articleService)
        {
            _articleService = articleService;
        }

        [HttpGet]
        [Route("index")]
        public async Task<IActionResult> IndexArticleByPath([FromQuery]string path)
        {
            var result = await _articleService.IndexArticleByPath(path);

            return Ok(result);
        }

        [HttpGet]
        [Route("search")]
        public async Task<IActionResult> SearchArticles([FromQuery]string query)
        {
            var result = await _articleService.SearchArticles(query);

            return Ok(result);
        }
    }
}