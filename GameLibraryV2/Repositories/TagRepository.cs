using GameLibraryV2.Data;
using GameLibraryV2.Interfaces;
using GameLibraryV2.Models;

namespace GameLibraryV2.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly DataContext dataContext;

        public TagRepository(DataContext context)
        {
            dataContext = context;
        }

        public IList<Game> GetGamesByTag(int tagId)
        {
            return dataContext.Games.Where(g => g.Tags.Any(g => g.Id == tagId)).Select(g => new Game
            {
                Id = g.Id,
                Name = g.Name,
                PicturePath = g.PicturePath,
                ReleaseDate = g.ReleaseDate,
                Description = g.Description,
                AgeRating = g.AgeRating,
                NSFW = g.NSFW,
                Type = g.Type,
                AveragePlayTime = g.AveragePlayTime,
                Rating = g.Rating,
                Developers = g.Developers.Select(t => new Developer
                {
                    Id = t.Id,
                    Name = t.Name,
                }).ToList(),
                Publishers = g.Publishers.Select(t => new Publisher
                {
                    Id = t.Id,
                    Name = t.Name,
                }).ToList(),
                Platforms = g.Platforms.Select(t => new Platform
                {
                    Id = t.Id,
                    Name = t.Name,
                }).ToList(),
                Genres = g.Genres.Select(t => new Genre
                {
                    Id = t.Id,
                    Name = t.Name,
                }).ToList(),
                Tags = g.Tags.Select(t => new Tag
                {
                    Id = t.Id,
                    Name = t.Name,
                }).ToList(),
            }).ToList();
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

        //--------------------------------------------

        public bool CreateTag(Tag tag)
        {
            dataContext.Add(tag);
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
