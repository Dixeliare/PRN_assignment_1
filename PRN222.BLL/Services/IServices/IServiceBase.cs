using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PRN222.BLL.Services.IServices
{
    public interface IServiceBase<T>
    {
        Task Create(T entity);
        Task<IEnumerable<T>> ReadAll();
        Task<T> ReadByCondition(Expression<Func<T, bool>> expression); 
        Task Update(string id, T entity);
        Task Delete(string id);

    }
}
