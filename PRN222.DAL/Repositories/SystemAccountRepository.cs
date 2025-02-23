using Microsoft.EntityFrameworkCore;
using PRN222.DAL.Models;
using PRN222.DAL.Repositories.IRepository;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PRN222.DAL.Repositories
{
    public class SystemAccountRepository : ISystemAccountRepository
    {
        private readonly PRN222Context? _context;

        public SystemAccountRepository(PRN222Context? context)
        {
            _context = context;
        }

        public async Task Add(SystemAccount entity)
        {
            await _context!.SystemAccounts.AddAsync(entity);
            await _context.SaveChangesAsync();
        }      

        public async Task<IEnumerable<SystemAccount>> GetAll()
        {
            return await _context!.SystemAccounts.ToListAsync();
        }

        public async Task<SystemAccount?> GetByCondition(Expression<Func<SystemAccount, bool>> expression)
        {
            return await _context!.SystemAccounts.FirstOrDefaultAsync(expression);
        }

        public async Task Update(SystemAccount entity)
        {
            var user = await GetByCondition(c => c.AccountId == entity.AccountId);
            if (user != null)
            { 
                user.AccountName = entity.AccountName;
                user.AccountPassword = entity.AccountPassword;
                user.AccountEmail = entity.AccountEmail;
                user.AccountRole = entity.AccountRole;
                await _context!.SaveChangesAsync();
            }
        }

        public async Task DeleteAccount(SystemAccount entity)
        { 
            var user = await GetByCondition(c => c.AccountId == entity.AccountId);
            if (user != null)
            {
                _context!.SystemAccounts.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
    }
}
