using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PRN222.BLL.Services.IServices;
using PRN222.DAL.Models;
using PRN222.Entity.DTOs;

namespace PRN222.Controllers
{
    public class SystemAccountController : Controller
    {
        private readonly ISystemAccountService _ser;
        private readonly AdminAccount _adminAccount;

        // Inject IOptions<AdminAccount> to load admin credentials
        public SystemAccountController(ISystemAccountService ser, IOptions<AdminAccount> adminAccount)
        {
            _ser = ser;
            _adminAccount = adminAccount.Value;
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
                // Check if user is Admin (from appsettings.json)
                if (acc.AccountName == _adminAccount.Username && acc.AccountPassword == _adminAccount.Password)
                {
                    HttpContext.Session.SetString("AccountId", "0"); // Special ID for Admin
                    HttpContext.Session.SetString("AccountRole", "99"); // Admin Role
                    HttpContext.Session.SetString("IsAdmin", "true");

                    return RedirectToAction("Index", "Admin"); // Redirect to Admin Dashboard
                }

                // Check if user exists in the database
                SystemAccount user = await _ser.ReadByCondition(a => a.AccountName == acc.AccountName && a.AccountPassword == acc.AccountPassword);

                if (user != null)
                {
                    HttpContext.Session.SetString("AccountId", user.AccountId.ToString());
                    HttpContext.Session.SetString("AccountRole", user.AccountRole.ToString());
                    HttpContext.Session.SetString("IsAdmin", "false");

                    // Redirect based on role
                    return user.AccountRole switch
                    {
                        1 => RedirectToAction("Dashboard", "Staff"),
                        2 => RedirectToAction("NewsPage", "NewsArticle"),
                        _ => RedirectToAction("NewsPage", "NewsArticle")
                    };
                }

                ModelState.AddModelError("", "Invalid username or password.");
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
