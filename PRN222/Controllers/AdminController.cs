using Microsoft.AspNetCore.Mvc;

namespace PRN222.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
