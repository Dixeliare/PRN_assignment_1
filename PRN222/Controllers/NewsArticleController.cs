using Microsoft.AspNetCore.Mvc;
using PRN222.BLL.Services.IServices;
using PRN222.DAL.Models;
using PRN222.Entity.DTOs;

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
        [RoleAuthorize(RequiredRole = "2")]
        public async Task<IActionResult> LecturerPage()
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
        public async Task<IActionResult> CreateNewsArticle(NewsArticleDTO news)
        {

            string? userId = HttpContext.Session.GetString("AccountId");
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "SystemAccount");
            }
            // Chuyển đổi userId sang short (hoặc thay đổi kiểu dữ liệu của CreatedById cho phù hợp)
            //news.CreatedById = short.Parse(userId);
            if (ModelState.IsValid)
            {
                await _ser.Create2(news);
                return RedirectToAction("ManageNewsArticle", "NewsArticle");
            }
            return View(news);
        }
    }
}
