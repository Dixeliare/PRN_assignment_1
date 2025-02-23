using Microsoft.AspNetCore.Mvc;
using PRN222.BLL.Services.IServices;
using PRN222.DAL.Models;

namespace PRN222.Controllers
{
    public class StaffController : Controller
    {
        private readonly INewsArticleService _newsService;
        private readonly ISystemAccountService _systemAccountService;
        private readonly ICategoryService _ser;

        public StaffController(INewsArticleService newsService, ISystemAccountService systemAccountService, ICategoryService ser)
        {
            _newsService = newsService;
            _systemAccountService = systemAccountService;
            _ser = ser;
        }
        [RoleAuthorize(RequiredRole = "1")]
        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetString("AccountId") == null)
            {
                return RedirectToAction("Login", "SystemAccount");
            }
            return View();
        }
        [HttpGet]
        [RoleAuthorize(RequiredRole = "1")]
        public async Task<IActionResult> ManageCategory()
        {
            if (HttpContext.Session.GetString("AccountId") == null)
            {
                return RedirectToAction("Login", "SystemAccount");
            }
            var categories = await _ser.ReadAll();
            return View(categories);
        }
        [RoleAuthorize(RequiredRole = "1")]
        public async Task<IActionResult> EditCategory(short id)
        {
            if (HttpContext.Session.GetString("AccountId") == null)
            {
                return RedirectToAction("Login", "SystemAccount");
            }
            var category = await _ser.ReadByCondition(c => c.CategoryId == id);
            return View(category);
        }

        [HttpPost]
        [RoleAuthorize(RequiredRole = "1")]
        public async Task<IActionResult> EditCategory(string id, Category category)
        {
            if (HttpContext.Session.GetString("AccountId") == null)
            {
                return RedirectToAction("Login", "SystemAccount");
            }
            if (ModelState.IsValid)
            {
                await _ser.Update(id, category);
                return RedirectToAction("ManageCategory");
            }
            return View(category);
        }
        [RoleAuthorize(RequiredRole = "1")]
        public async Task<IActionResult> DeleteCategory(short id)
        {
            if (HttpContext.Session.GetString("AccountId") == null)
            {
                return RedirectToAction("Login", "SystemAccount");
            }
            var category = await _ser.ReadByCondition(c => c.CategoryId == id);
            return View(category);
        }

        [HttpPost, ActionName("DeleteCategory")]
        [RoleAuthorize(RequiredRole = "1")]
        public async Task<IActionResult> DeleteConfirmed(short id)
        {
            if (HttpContext.Session.GetString("AccountId") == null)
            {
                return RedirectToAction("Login", "SystemAccount");
            }
            var newId = id.ToString();
            var category = await _ser.ReadByCondition(c => c.CategoryId == id);
            if (category != null)
            {

                await _ser.Delete(newId);
            }
            return RedirectToAction("ManageCategory");
        }
        [RoleAuthorize(RequiredRole = "1")]
        public IActionResult CreateCategory()
        {
            if (HttpContext.Session.GetString("AccountId") == null)
            {
                return RedirectToAction("Login", "SystemAccount");
            }
            return View();
        }

        [HttpPost]
        [RoleAuthorize(RequiredRole = "1")]
        public async Task<IActionResult> CreateCategory(Category category)
        {
            if (HttpContext.Session.GetString("AccountId") == null)
            {
                return RedirectToAction("Login", "SystemAccount");
            }
            if (ModelState.IsValid)
            {
                await _ser.Create(category);
                return RedirectToAction("ManageCategory");
            }
            return View(category);
        }
        [RoleAuthorize(RequiredRole = "1")]
        public async Task<IActionResult> Profile()
        {
            if (HttpContext.Session.GetString("AccountId") == null)
            {
                return RedirectToAction("Login", "SystemAccount");
            }
            string userIdString = HttpContext.Session.GetString("AccountId");
            int userId = int.Parse(userIdString);

            var user = await _systemAccountService.GetByAccountId(userId);

            return View(user);
        }
        [RoleAuthorize(RequiredRole = "1")]
        public async Task<IActionResult> EditProfile()
        {
            if (HttpContext.Session.GetString("AccountId") == null)
            {
                return RedirectToAction("Login", "SystemAccount");
            }
            string userIdString = HttpContext.Session.GetString("AccountId");
            int userId = int.Parse(userIdString);
            var user = await _systemAccountService.GetByAccountId(userId);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        [RoleAuthorize(RequiredRole = "1")]
        public async Task<IActionResult> SaveProfile(SystemAccount updatedAccount)
        {
            if (HttpContext.Session.GetString("AccountId") == null)
            {
                return RedirectToAction("Login", "SystemAccount");
            }
            string userIdString = HttpContext.Session.GetString("AccountId");
            int userId = int.Parse(userIdString);
            var account = await _systemAccountService.GetByAccountId(userId);

            if (account == null)
            {
                return NotFound();
            }

            // Update properties
            account.AccountName = updatedAccount.AccountName;
            account.AccountEmail = updatedAccount.AccountEmail;

            await _systemAccountService.Update(userIdString, account);

            TempData["SuccessMessage"] = "Profile updated successfully!";
            return RedirectToAction("Profile");
        }

        [RoleAuthorize(RequiredRole = "1")]
        public async Task<IActionResult> NewsHistory()
        {
            if (HttpContext.Session.GetString("AccountId") == null)
            {
                return RedirectToAction("Login", "SystemAccount");
            }
            int userId = int.Parse(HttpContext.Session.GetString("AccountId"));

            var newsArticles = await _newsService.ReadByCreatedId(userId);

            return View(newsArticles);
        }

    }
}
