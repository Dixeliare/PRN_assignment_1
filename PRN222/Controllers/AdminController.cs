using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            var isAdmin = HttpContext.Session.GetString("IsAdmin");
            if (string.IsNullOrEmpty(isAdmin) || !isAdmin.Equals("true"))
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
        public async Task<IActionResult> Create(SystemAccount account)
        {
            if (ModelState.IsValid)
            {
                await _systemAccountService.Create(account);
                return RedirectToAction("Index");
            }
            return View(account);
        }
        public async Task<IActionResult> Edit(short id)
        {
            var account = await _systemAccountService.GetByAccountId(id);
            if (account == null)
            {
                return NotFound();
            }
            return View(account);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SystemAccount account)
        {
            if (ModelState.IsValid)
            {
                await _systemAccountService.Update(account.AccountId.ToString(),account);
                return RedirectToAction("Index");
            }
            return View(account);
        }


        public async Task<IActionResult> Delete(int id)
        {
            var item = await _systemAccountService.ReadByCondition(a => a.AccountId == id);
            return View(item);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(short id)
        {
            SystemAccount account = await _systemAccountService.ReadByCondition(c => c.AccountId == id);
            if (account != null)
            {
                await _systemAccountService.Delete(id.ToString());
                return Json(new { success = true, message = "Account deleted successfully" });
                
            }
            return Json(new { success = false, message = "Account not found" });
        }

    }
}
