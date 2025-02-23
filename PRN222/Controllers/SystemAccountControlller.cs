using Microsoft.AspNetCore.Mvc;
using PRN222.BLL.Services.IServices;

namespace PRN222.Controllers
{
    public class SystemAccountControlller : Controller
    {
        private readonly ISystemAccountService _ser;

        public SystemAccountControlller(ISystemAccountService ser)
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
