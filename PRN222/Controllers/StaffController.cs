using Microsoft.AspNetCore.Mvc;

namespace PRN222.Controllers
{
    public class StaffController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
