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
    public class NewsArticleRepository : INewsArticleRepository
    {
        private readonly PRN222Context _context;

        public NewsArticleRepository(PRN222Context context)
        {
            _context = context;
        }

        public async Task Add(NewsArticle entity)
        {
            await _context.NewsArticles.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteNewsArticle(string id)
        {
            var news = await GetByCondition(n => n.NewsArticleId == id);
            if (news != null)
            {
                _context.NewsArticles.Remove(news);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<NewsArticle>> GetAll()
        {
            return await _context.NewsArticles.Include(c => c.Category).ToListAsync();
        }

        public async Task<NewsArticle?> GetByCondition(Expression<Func<NewsArticle, bool>> expression)
        {
            return await _context.NewsArticles.FirstOrDefaultAsync(expression);
        }

        public async Task Update(NewsArticle entity)
        {
            var news = await GetByCondition(n => n.NewsArticleId == entity.NewsArticleId);
            if (news != null)
            {
                news.NewsTitle = entity.NewsTitle;
                news.Headline = entity.Headline;
                news.NewsContent = entity.NewsContent;
                news.NewsSource = entity.NewsSource;
                news.CategoryId = entity.CategoryId;
                news.NewsStatus = entity.NewsStatus;
                news.UpdatedById = entity.UpdatedById;
                news.ModifiedDate = entity.ModifiedDate;
            }
        }
    }
}
