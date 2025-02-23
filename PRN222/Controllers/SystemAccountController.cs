using Microsoft.AspNetCore.Mvc;
using PRN222.BLL.Services.IServices;
using PRN222.DAL.Models;
using PRN222.Entity.DTOs;

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

        [HttpPost]
        public async Task<IActionResult> Login(SystemAccountDTO acc)
        {
            if (ModelState.IsValid)
            {
                SystemAccount user = await _ser.ReadByCondition(a => a.AccountName == acc.AccountName && a.AccountPassword == acc.AccountPassword);
                if (user != null)
                {
                    if (user.AccountRole.Equals(1))
                    {
                        return RedirectToAction("Dashboard", "Staff");
                    } else
                    if (user.AccountRole.Equals(2))
                    {
                        return RedirectToAction("NewsPage", "NewsArticle");
                    }
                    else
                    {
                        return RedirectToAction("NewsPage", "NewsArticle");
                    }
                }
                ModelState.AddModelError("", "Tên tài khoản hoặc mật khẩu không đúng.");
            }
            return View(acc);

        }
    }
}
