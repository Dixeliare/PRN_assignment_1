﻿using PRN222.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN222.DAL.Repositories.IRepository
{
    public interface ISystemAccountRepository : IRepositoryBase<SystemAccount>
    {
        Task DeleteAccount(SystemAccount entity);
    }
}
