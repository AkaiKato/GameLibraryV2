using GameLibraryV2.Data;
using GameLibraryV2.Interfaces;
using GameLibraryV2.Models;

namespace GameLibraryV2.Repositories
{
    public class PlatformRepository : IPlatformRepository
    {
        private readonly DataContext dataContext;

        public PlatformRepository(DataContext context)
        {
            dataContext = context;
        }

        public IList<Game> GetGameByPlatform(int platformId)
        {
            return dataContext.Games.Where(p => p.Platforms.Any(p => p.Id == platformId)).Select(g => new Game
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

        public Platform GetPlatformById(int platformId)
        {
            return dataContext.Platforms.Where(p => p.Id == platformId).FirstOrDefault()!;
        }

        public Platform GetPlatformByName(string name)
        {
            return dataContext.Platforms.Where(p => p.Name.ToLower() == name.ToLower()).FirstOrDefault()!;
        }

        public IList<Platform> GetPlatforms()
        {
            return dataContext.Platforms.OrderBy(p => p.Id).ToList();
        }

        public bool PlatformExist(int platformId)
        {
            return dataContext.Platforms.Any(p => p.Id == platformId);
        }
    }
}
