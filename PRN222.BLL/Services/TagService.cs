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
    public class TagService : ITagService
    {
        private readonly ITagRepository _repos;

        public TagService(ITagRepository repos)
        { 
            _repos = repos;
        }

        public async Task Create(Tag entity)
        {
            var tags = await _repos.GetAll();
            int newTagId = 1;
            foreach (var item in tags)
            {
                if (item.TagId == newTagId)
                {
                    newTagId += 1;
                }
                else break;
            }

            entity.TagId = newTagId;

            var tag = new Tag
            {
                TagId = entity.TagId,
                TagName = entity.TagName,
                Note = entity.Note,
            };

            if (tag.TagName is not string || tag.Note is not string) throw new ArgumentException("Giá trị của tag không hợp lệ.");

            await _repos.Add(tag);
        }

        public async Task Delete(string id)
        {
            var tag = await ReadByCondition(t => t.TagId == ConvertMethod.ConvertStringToInt(id));
            if (tag != null)
            {
                await _repos.DeleteTag(tag);
            }
        }

        public async Task<IEnumerable<Tag>> ReadAll()
        {
            return await _repos.GetAll();
        }

        public async Task<Tag> ReadByCondition(Expression<Func<Tag, bool>> expression)
        {
            return await _repos.GetByCondition(expression);
        }

        public async Task Update(string id, Tag entity)
        {
            int newId = ConvertMethod.ConvertStringToInt(id);
            var tag = await ReadByCondition(t => t.TagId == newId);
            if (tag != null)
            {
                tag.TagName = entity.TagName;
                tag.Note = entity.Note;
            }
        }
    }
}
