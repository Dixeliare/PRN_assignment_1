using Microsoft.AspNetCore.Mvc;
using PRN222.BLL.Services.IServices;
using PRN222.DAL.Models;

namespace PRN222.Controllers
{
    public class AdminController : Controller
    {
        private readonly ISystemAccountService _systemAccountService;

        public AdminController(ISystemAccountService systemAccountService)
        {
            _systemAccountService = systemAccountService;
        }

        public async Task<IActionResult> Index()
        {
            if (!HttpContext.Session.GetString("IsAdmin").Equals("true"))
            {
                return RedirectToAction("Login", "SystemAccount");
            }
            var accounts = await _systemAccountService.ReadAll(); // ✅ Ensure the method is awaited

            return View(accounts);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(SystemAccount account)
        {
            if (ModelState.IsValid)
            {
                _systemAccountService.Create(account);
                return RedirectToAction("Index");
            }
            return View(account);
        }
        public IActionResult Edit(short id)
        {
            var account = _systemAccountService.GetByAccountId(id);
            if (account == null)
            {
                return NotFound();
            }
            return View(account);
        }

        [HttpPost]
        public IActionResult Edit(SystemAccount account)
        {
            if (ModelState.IsValid)
            {
                _systemAccountService.Update(account.AccountId.ToString(),account);
                return RedirectToAction("Index");
            }
            return View(account);
        }
        /// ✅ **Updated Delete Method to Work with AJAX**
        [HttpPost]
        public async Task<IActionResult> Delete(short id)
        {
            var account = await _systemAccountService.GetByAccountId(id);
            if (account == null)
            {
                return Json(new { success = false, message = "Account not found" });
            }

            await _systemAccountService.Delete(id.ToString());
            return Json(new { success = true, message = "Account deleted successfully" });
        }

    }
}
