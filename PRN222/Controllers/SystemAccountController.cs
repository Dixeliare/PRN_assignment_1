using Microsoft.AspNetCore.Mvc;
using PRN222.BLL.Services.IServices;

namespace PRN222.Controllers
{
    public class SystemAccountController : Controller
    {
        private readonly ISystemAccountService _ser;

        public SystemAccountController(ISystemAccountService ser)
        {
            _ser = ser;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

    }
}
