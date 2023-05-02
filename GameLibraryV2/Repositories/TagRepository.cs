using GameLibraryV2.Data;
using GameLibraryV2.Interfaces;
using GameLibraryV2.Models.Common;

namespace GameLibraryV2.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly DataContext dataContext;

        public TagRepository(DataContext context)
        {
            dataContext = context;
        }

        public Tag GetTagById(int tagId)
        {
            return dataContext.Tags.Where(t => t.Id == tagId).FirstOrDefault()!;
        }

        public Tag GetTagByName(string tagName)
        {
            return dataContext.Tags.Where(t => t.Name.ToLower() == tagName.ToLower()).FirstOrDefault()!;
        }

        public IList<Tag> GetTags()
        {
            return dataContext.Tags.OrderBy(t => t.Id).ToList();
        }

        public bool TagExists(int tagId)
        {
            return dataContext.Tags.Any(t => t.Id == tagId);
        }

        public bool TagNameAlreadyInUse(int tagId, string tagName)
        {
            return dataContext.Tags.Any(t => t.Name.Trim().ToLower() == tagName.Trim().ToLower() && t.Id != tagId);
        }

        //--------------------------------------------

        public bool CreateTag(Tag tag)
        {
            dataContext.Add(tag);
            return Save();
        }

        public bool UpdateTag(Tag tag)
        {
            dataContext.Update(tag);
            return Save();
        }

        public bool DeleteTag(Tag tag)
        {
            dataContext.Remove(tag);
            return Save();
        }

        private bool Save()
        {
            var saved = dataContext.SaveChanges();
            //var saved = 1;
            return saved > 0 ? true : false;
        }
    }
}
