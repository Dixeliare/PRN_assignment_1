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
                    // Store user details in session
                    HttpContext.Session.SetString("UserId", user.AccountId.ToString());
                    HttpContext.Session.SetString("UserName", user.AccountName);
                    HttpContext.Session.SetString("UserRole", user.AccountRole.ToString());

                    // Redirect based on role
                    return user.AccountRole switch
                    {
                        1 => RedirectToAction("Dashboard", "Staff"),
                        2 => RedirectToAction("NewsPage", "NewsArticle"),
                        _ => RedirectToAction("NewsPage", "NewsArticle")
                    };
                }

                ModelState.AddModelError("", "Tên tài khoản hoặc mật khẩu không đúng.");
            }
            return View(acc);
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Remove all session data
            return RedirectToAction("Login");
        }
    }
}
