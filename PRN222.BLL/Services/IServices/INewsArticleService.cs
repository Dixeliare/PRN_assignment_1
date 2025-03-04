﻿using PRN222.DAL.Models;
using PRN222.Entity.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN222.BLL.Services.IServices
{
    public interface INewsArticleService : IServiceBase<NewsArticle>
    {
        Task<IEnumerable<NewsArticle>> ReadByCreatedId(int userId);
        Task Create2(NewsArticleDTO entity);
    }
}
