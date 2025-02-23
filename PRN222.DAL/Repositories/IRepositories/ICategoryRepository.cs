using PRN222.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN222.DAL.Repositories.IRepository
{
    public interface ICategoryRepository : IRepositoryBase<Category>
    {
        Task DeleteParentCategory(Category entity);
        Task<IEnumerable<Category>> GetSubCategoriesByParentCategoryId(short parentCategoryId);
    }
}
