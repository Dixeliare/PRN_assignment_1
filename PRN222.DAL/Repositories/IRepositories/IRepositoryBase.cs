using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PRN222.DAL.Repositories.IRepository
{
    public interface IRepositoryBase<T>
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetByCondition(Expression<Func<T, bool>> expression);
        Task Add(T entity);
        Task Update(T entity);

    }
}
