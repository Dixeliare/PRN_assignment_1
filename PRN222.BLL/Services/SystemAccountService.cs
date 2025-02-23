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
    public class SystemAccountService : ISystemAccountService
    {
        private readonly ISystemAccountRepository _repos;

        public SystemAccountService(ISystemAccountRepository repos)
        {
            _repos = repos;
        }

        public async Task Create(SystemAccount entity)
        {
            var accounts = await _repos.GetAll();
            short newAccountId = 1;
            foreach (var item in accounts)
            {
                if (item.AccountId == newAccountId)
                {
                    newAccountId += 1;
                }
                else break;
            }

            entity.AccountId = newAccountId;

            var account = new SystemAccount
            {
                AccountId = entity.AccountId,
                AccountName = entity.AccountName,
                AccountEmail = entity.AccountEmail,
                AccountRole = entity.AccountRole,
                AccountPassword = entity.AccountPassword,
            };

            if (account.AccountName is not string 
                || account.AccountEmail is not string 
                || account.AccountRole is not int 
                || account.AccountPassword is not string)
                throw new ArgumentException("Giá trị của account không hợp lệ.");

            await _repos.Add(account);
        }

        public async Task Delete(string id)
        {
            var account = await ReadByCondition(a => a.AccountId == ConvertMethod.ConvertStringToShort(id));
            if (account != null)
            {
                await _repos.DeleteAccount(account);
                return;
            }
            else
            {
                throw new ArgumentException("Account không tồn tại để xóa.");
            }
        }

        public async Task<IEnumerable<SystemAccount>> ReadAll()
        {
            return await _repos.GetAll();
        }

        public async Task<SystemAccount> GetByAccountId(int id)
        {

            return await _repos.GetByCondition(a => a.AccountId == id);
        }

        public async Task<SystemAccount> ReadByCondition(Expression<Func<SystemAccount, bool>> expression)
        {
            return await _repos.GetByCondition(expression);
        }

        public async Task Update(string id, SystemAccount entity)
        {
            var account = await ReadByCondition(a => a.AccountId == ConvertMethod.ConvertStringToShort(id));
            if (account != null)
            {
                account.AccountName = entity.AccountName;
                account.AccountEmail = entity.AccountEmail;
                account.AccountRole = entity.AccountRole;
                account.AccountPassword = entity.AccountPassword;
                await _repos.Update(account);
            }
            else throw new ArgumentException("Giá trị nhập không hợp lệ.");
        }
    }
}
