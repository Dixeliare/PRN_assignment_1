using PRN222.BLL.Services.IServices;
using PRN222.DAL.Models;
using PRN222.DAL.Repositories.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PRN222.BLL.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repos;

        public CategoryService(ICategoryRepository repos)
        {
            _repos = repos;
        }

        public async Task Create(Category entity)
        {
            //var categories = await _repos.GetAll();
            //short newCategoryId = 1;
            //foreach (var item in categories)
            //{
            //    if (item.CategoryId == newCategoryId)
            //    {
            //        newCategoryId += 1;
            //    }
            //    else break;
            //}

            //entity.CategoryId = newCategoryId;

            var category = new Category
            {
                CategoryId = entity.CategoryId,
                CategoryName = entity.CategoryName,
                CategoryDesciption = entity.CategoryDesciption,
                ParentCategoryId = entity.ParentCategoryId,
                IsActive = entity.IsActive,
            };
            if (category.CategoryName is not string
                || category.CategoryDesciption is not string
                || category.IsActive is not bool)
                throw new ArgumentException("Giá trị của category không hợp lệ.");

            await _repos.Add(category);
        }

        public async Task Delete(string id)
        {
            var category = await ReadByCondition(c => c.CategoryId == ConvertMethod.ConvertStringToShort(id));
            if (category != null)
            {
                await _repos.DeleteParentCategory(category);
            }
            throw new ArgumentException("Category không tồn tại để xóa.");
        }

        public async Task<IEnumerable<Category>> ReadAll()
        {
            return await _repos.GetAll();
        }

        public async Task<Category> ReadByCondition(Expression<Func<Category, bool>> expression)
        {
            return await _repos.GetByCondition(expression);
        }

        public async Task<IEnumerable<Category>> ReadSubCategoriesByParentCategoryIdSer(string parentCategoryId)
        {
            short parentId = ConvertMethod.ConvertStringToShort(parentCategoryId);
            var allCategories = await _repos.GetAll();
            var subCategories = allCategories
                .Where(c => c.ParentCategoryId.HasValue && c.ParentCategoryId.Value == parentId)
                .ToList();

            return subCategories;
        }

        public async Task Update(string id, Category entity)
        {
            short newId = ConvertMethod.ConvertStringToShort(id);
            var category = await ReadByCondition(c => c.CategoryId == newId);
            if (category != null)
            {
                category.ParentCategoryId = entity.ParentCategoryId;
                category.CategoryName = entity.CategoryName;
                category.CategoryDesciption = entity.CategoryDesciption;
                category.IsActive = entity.IsActive;
                await _repos.Update(category);
            }
            else throw new ArgumentException("Giá trị nhập không hợp lệ");
        }
    }

    class ConvertMethod
    {
        internal static short ConvertStringToShort(string input)
        {
            if (short.TryParse(input, out short result))
            {
                return result;
            }
            else
            {
                throw new ArgumentException("Chuỗi nhập vào không hợp lệ và không thể chuyển đổi sang kiểu short.");
            }
        }

        internal static string ConvertIntToString(int input)
        {
            try
            {
                string result = input.ToString();
                if (string.IsNullOrEmpty(result))
                {
                    throw new ArgumentException("Kết quả chuyển đổi không hợp lệ.");
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Không thể chuyển đổi int sang string.", ex);
            }
        }

        internal static int ConvertStringToInt(string input)
        {
            if (int.TryParse(input, out int result))
            {
                return result;
            }
            else
            {
                throw new ArgumentException("Id phải là các số nguyên");
            }
        }
    }
}
