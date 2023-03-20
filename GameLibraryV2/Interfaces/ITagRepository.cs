using GameLibraryV2.Models;

namespace GameLibraryV2.Interfaces
{
    public interface ITagRepository
    {
        public Tag GetTagById(int tagId);

        public Tag GetTagByName(string tagName);

        public IList<Tag> GetTags();

        public IList<Game> GetGamesByTag(int tagId);

        public bool TagExists(int tagId);

        public bool CreateTag(Tag tag);
    }
}
