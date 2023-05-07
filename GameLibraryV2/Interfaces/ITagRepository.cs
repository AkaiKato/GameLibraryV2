using GameLibraryV2.Data;
using GameLibraryV2.Models.Common;

namespace GameLibraryV2.Interfaces
{
    public interface ITagRepository
    {
        public Task<Tag> GetTagByIdAsync(int tagId);

        public Task<Tag> GetTagByNameAsync(string tagName);

        public Task<IList<Tag>> GetTagsAsync();

        public Task<bool> TagExistsAsync(int tagId);

        public Task<bool> TagNameAlreadyInUseAsync(int tagId, string tagName);

        public void CreateTag(Tag tag);

        public void UpdateTag(Tag tag);

        public void DeleteTag(Tag tag);

        public Task SaveTagAsync();
    }
}
