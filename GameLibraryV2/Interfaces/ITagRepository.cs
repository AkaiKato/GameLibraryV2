using GameLibraryV2.Models;

namespace GameLibraryV2.Interfaces
{
    public interface ITagRepository
    {
        public Tag GetTagById(int tagId);

        public Tag GetTagByName(string tagName);

        public IList<Tag> GetTags();

        public bool TagExists(int tagId);

        public bool TagNameAlreadyInUse(int tagId, string tagName);

        public bool CreateTag(Tag tag);

        public bool UpdateTag(Tag tag);

        public bool DeleteTag(Tag tag);
    }
}
