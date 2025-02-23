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
    public class TagRepository : ITagRepository
    {
        private readonly PRN222Context _context;

        public TagRepository(PRN222Context context)
        {
            _context = context;
        }

        public async Task Add(Tag entity)
        {
            await _context.Tags.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Tag>> GetAll()
        {
            return await _context.Tags.ToListAsync();
        }

        public async Task<Tag?> GetByCondition(Expression<Func<Tag, bool>> expression)
        {
            return await _context.Tags.FirstOrDefaultAsync(expression);
        }

        public async Task Update(Tag entity)
        {
            var tag = await GetByCondition(t => t.TagId == entity.TagId);
            if (tag != null)
            {
                tag.TagName = entity.TagName;
                tag.Note = entity.Note;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteTag(Tag entity)
        {
            var tag = await GetByCondition(t => t.TagId == entity.TagId);
            if (tag != null)
            {
                _context.Tags.Remove(tag);
                await _context.SaveChangesAsync();
            }
        }
    }
}
