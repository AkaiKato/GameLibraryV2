using GameLibraryV2.Data;
using GameLibraryV2.Interfaces;
using GameLibraryV2.Models.Common;
using Microsoft.EntityFrameworkCore;

namespace GameLibraryV2.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly DataContext dataContext;

        public TagRepository(DataContext context)
        {
            dataContext = context;
        }

        public async Task<Tag> GetTagByIdAsync(int tagId)
        {
            return await dataContext.Tags.FirstOrDefaultAsync(t => t.Id == tagId)!;
        }

        public async Task<Tag> GetTagByNameAsync(string tagName)
        {
            return await dataContext.Tags.FirstOrDefaultAsync(t => t.Name.ToLower() == tagName.ToLower())!;
        }

        public async Task<IList<Tag>> GetTagsAsync()
        {
            return await dataContext.Tags.OrderBy(t => t.Id).ToListAsync();
        }

        public async Task<bool> TagExistsAsync(int tagId)
        {
            return await dataContext.Tags.AnyAsync(t => t.Id == tagId);
        }

        public async Task<bool> TagNameAlreadyInUseAsync(int tagId, string tagName)
        {
            return await dataContext.Tags.AnyAsync(t => t.Name.Trim().ToLower() == tagName.Trim().ToLower() && t.Id != tagId);
        }

        //--------------------------------------------

        public void CreateTag(Tag tag)
        {
            dataContext.Add(tag);
        }

        public void UpdateTag(Tag tag)
        {
            dataContext.Update(tag);
        }

        public void DeleteTag(Tag tag)
        {
            dataContext.Remove(tag);
        }

        public async Task SaveTagAsync()
        {
            await dataContext.SaveChangesAsync();
        }
    }
}
