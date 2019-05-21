using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApplication.Elastic;

namespace WebApplication.Controllers
{
    [Route("api/[controller]")]
    public class DocumentController : Controller
    {
        private readonly SearchService searchService;

        public DocumentController(SearchService searchService)
        {
            this.searchService = searchService;
        }

        [HttpGet("search")]
        public async Task<JsonResult> Search([FromQuery]string query, int page = 1, int pageSize = 10)
        {
            var result = await this.searchService.Search(query, page, pageSize);
            return Json(result);
        }

        [HttpGet("autocomplete")]
        public async Task<JsonResult> Autocomplete([FromQuery]string query)
        {
            var result = await this.searchService.Autocomplete(query);
            return Json(result);
        }
    }
}
