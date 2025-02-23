using Microsoft.AspNetCore.Mvc;
using PRN222.BLL.Services.IServices;
using PRN222.DAL.Models;

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

        public async Task<IActionResult> ManageNewsArticle()
        {
            var news = await _ser.ReadAll();
            return View(news);
        }

        public IActionResult CreateNewsArticle()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewsArticle(NewsArticle news)
        {
            if (ModelState.IsValid)
            {
                await _ser.Create(news);
                return RedirectToAction("ManageNewsArticle");
            }
            return View(news);
        }
    }
}
