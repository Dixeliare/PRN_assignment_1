using Microsoft.EntityFrameworkCore;
using PRN222.DAL.Models;
using PRN222.DAL.Repositories.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PRN222.DAL.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly PRN222Context _context;

        public CategoryRepository(PRN222Context context)
        {
            _context = context;
        }

        public async Task Add(Category entity)
        {
            await _context.Categories.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteParentCategory(Category entity)
        {
            var parentCategory = await GetByCondition(c => c.ParentCategoryId == entity.ParentCategoryId);
            if (parentCategory != null)
            {
                _context.Remove(parentCategory);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Category?>> GetAll()
        {

            return await _context.Categories.OrderBy(c => c.CategoryId).ToListAsync();
        }

        public async Task<IEnumerable<Category>> GetSubCategoriesByParentCategoryId(short parentCategoryId)
        {
            return await _context.Categories.Where(c => c.ParentCategoryId == parentCategoryId).ToListAsync();
        }

        public async Task<Category?> GetByCondition(Expression<Func<Category, bool>> expression)
        {
            return await _context.Categories.FirstOrDefaultAsync(expression);
        }

        public async Task Update(Category entity)
        {
            var category = await GetByCondition(e => e.CategoryId == entity.CategoryId);
            if (category != null)
            {
                category.CategoryName = entity.CategoryName;
                category.CategoryDesciption = entity.CategoryDesciption;
                category.IsActive = entity.IsActive;
                category.ParentCategoryId = entity.ParentCategoryId;

            }
        }
    }
}
