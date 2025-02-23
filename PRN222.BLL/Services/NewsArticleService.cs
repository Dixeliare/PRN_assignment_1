using PRN222.BLL.Services.IServices;
using PRN222.DAL.Models;
using PRN222.DAL.Repositories.IRepository;
using PRN222.Entity.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PRN222.BLL.Services
{

    public class NewsArticleService : INewsArticleService
    {
        private readonly INewsArticleRepository _repos;

        public NewsArticleService(INewsArticleRepository repos)
        {
            _repos = repos;
        }
        public async Task Create2(NewsArticleDTO entity)
        {
            var articles = await _repos.GetAll();
            int newId = 1;
            var sortedArticles = articles
                .OrderBy(a => int.Parse(a.NewsArticleId))
                .ToList();
            foreach (var article in sortedArticles)
            {
                int currentId = int.Parse(article.NewsArticleId);
                if (currentId == newId)
                {
                    newId++;
                }
                else
                {
                    break;
                }
            }

            // Cập nhật giá trị mới cho DTO
            //entity.NewsArticleId = newId.ToString();
            entity.CreatedDate = DateTime.Now;

            //// Kiểm tra các trường bắt buộc
            //if (string.IsNullOrWhiteSpace(entity.NewsTitle) ||
            //    string.IsNullOrWhiteSpace(entity.Headline) ||
            //    string.IsNullOrWhiteSpace(entity.NewsContent) ||
            //    string.IsNullOrWhiteSpace(entity.NewsSource) ||
            //    !entity.CategoryId.HasValue ||
            //    !entity.NewsStatus.HasValue ||
            //    !entity.CreatedById.HasValue)
            //{
            //    throw new ArgumentException("Một trong số các giá trị của NewsArticle không hợp lệ.");
            //}

            //// Mapping thủ công từ DTO sang entity
            NewsArticle data = new NewsArticle
            {
                //NewsArticleId = entity.NewsArticleId,
                NewsTitle = entity.NewsTitle,
                Headline = entity.Headline,
                CreatedDate = entity.CreatedDate,
                NewsContent = entity.NewsContent,
                NewsSource = entity.NewsSource,
                CategoryId = entity.CategoryId,
                NewsStatus = entity.NewsStatus,
                CreatedById = entity.CreatedById,
                UpdatedById = null,
                ModifiedDate = null
            };

            // Thêm entity vào database
            await _repos.Add(data);
        }

        public async Task Create(NewsArticle entity)
        {
            var articles = await _repos.GetAll();
            int newId = 1;
            var sortedArticles = articles
                .OrderBy(a => int.Parse(a.NewsArticleId))
                .ToList();
            foreach (var article in sortedArticles)
            {
                int currentId = int.Parse(article.NewsArticleId);
                if (currentId == newId)
                {
                    newId++;
                }
                else
                {
                    break;
                }
            }

            entity.NewsArticleId = newId.ToString();
            entity.CreatedDate = DateTime.Now;

            // Nếu muốn kiểm tra các trường bắt buộc, bạn có thể kiểm tra như sau:
            if (string.IsNullOrWhiteSpace(entity.NewsTitle) ||
                string.IsNullOrWhiteSpace(entity.Headline) ||
                string.IsNullOrWhiteSpace(entity.NewsContent) ||
                string.IsNullOrWhiteSpace(entity.NewsSource) ||
                !entity.CategoryId.HasValue ||
                !entity.NewsStatus.HasValue ||
                !entity.CreatedById.HasValue)
            {
                throw new ArgumentException("Một trong số các giá trị của NewsArticle không hợp lệ.");
            }

            // Thêm entity vào database
            await _repos.Add(entity);
        }

        public async Task Delete(string id)
        {
            ConvertMethod.ConvertStringToInt(id);
            var news = await ReadByCondition(n => n.NewsArticleId == id);
            if (news != null)
            {
                await _repos.DeleteNewsArticle(id);
            }
            else throw new ArgumentException("NewsArticle không tồn tại để xóa.");
        } 

        public async Task<IEnumerable<NewsArticle>> ReadAll()
        {
            return await _repos.GetAll();
        }

        public async Task<NewsArticle> ReadByCondition(Expression<Func<NewsArticle, bool>> expression)
        {
            return await _repos.GetByCondition(expression);
        }

        public async Task<IEnumerable<NewsArticle>> ReadByCreatedId(int userId)
        {
            var allNews = await _repos.GetAll();
            return allNews.Where(n => n.CreatedById == userId).ToList();
        }

        public async Task Update(string id, NewsArticle entity)
        {
            ConvertMethod.ConvertStringToInt(id);
            var news = await ReadByCondition(n => n.NewsArticleId == id);
            if (news != null)
            {
                news.NewsTitle = entity.NewsTitle;
                news.Headline = entity.Headline;
                news.NewsContent = entity.NewsContent;
                news.NewsSource = entity.NewsSource;
                news.CategoryId = entity.CategoryId;
                news.NewsStatus = entity.NewsStatus;
                news.UpdatedById = entity.UpdatedById;
                news.UpdatedById = entity.UpdatedById;
                await _repos.Update(news);
            }
            else throw new ArgumentException("Giá trị nhập không hợp lệ.");
        }
    }
}
