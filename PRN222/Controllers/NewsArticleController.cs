using Microsoft.AspNetCore.Mvc;
using PRN222.BLL.Services.IServices;

namespace PRN222.Controllers
{
    public class NewsArticleController : Controller
    {
        private readonly INewsArticleService _ser;

        public NewsArticleController(INewsArticleService ser)
        {
            _ser = ser;
        }

        public async Task<IActionResult> NewsPage()
        {
            var news = await _ser.ReadAll();
            return View(news);
        }
    }
}
