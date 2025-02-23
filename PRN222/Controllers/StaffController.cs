using Microsoft.AspNetCore.Mvc;
using PRN222.BLL.Services.IServices;
using PRN222.DAL.Models;
using PRN222.Entity.DTOs;

namespace PRN222.Controllers
{
    public class StaffController : Controller
    {
        private readonly ICategoryService _ser;

        public StaffController(ICategoryService ser)
        {
            _ser = ser;
        }
        public IActionResult Dashboard()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ManageCategory()
        {
            var categories = await _ser.ReadAll();
            return View(categories);
        }

        public async Task<IActionResult> EditCategory(short id)
        { 
            var category = await _ser.ReadByCondition(c => c.CategoryId == id);
            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> EditCategory(string id,Category category)
        {
            if (ModelState.IsValid)
            {
                await _ser.Update(id, category);
                return RedirectToAction("ManageCategory");
            }
            return View(category);
        }

        public async Task<IActionResult> DeleteCategory(short id)
        {
            var category = await _ser.ReadByCondition(c => c.CategoryId == id);
            return View(category);
        }

        [HttpPost, ActionName("DeleteCategory")]
        public async Task<IActionResult> DeleteConfirmed(short id)
        {
            var newId = id.ToString();
            var category = await _ser.ReadByCondition(c => c.CategoryId == id);
            if (category != null)
            {

                await _ser.Delete(newId);
            }
            return RedirectToAction("ManageCategory");
        }

        public IActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                await _ser.Create(category);
                return RedirectToAction("ManageCategory");
            }
            return View(category);
        }
    }    
}
