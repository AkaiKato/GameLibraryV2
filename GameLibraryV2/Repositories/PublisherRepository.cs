using GameLibraryV2.Data;
using GameLibraryV2.Interfaces;
using GameLibraryV2.Models;

namespace GameLibraryV2.Repositories
{
    public class PublisherRepository : IPublisherRepository
    {
        private readonly DataContext dataContext;

        public PublisherRepository(DataContext context)
        {
            dataContext = context;
        }
        public IList<Game> GetGamesByPublisher(int publisherId)
        {
            return dataContext.Games.Where(g => g.Publishers.Any(p => p.Id == publisherId)).Select(g => new Game
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

        public Publisher GetPublisherById(int publisherId)
        {
            return dataContext.Publishers.Where(p => p.Id == publisherId).FirstOrDefault()!;
        }

        public Publisher GetPublisherByName(string publisherName)
        {
            return dataContext.Publishers.Where(p => p.Name.ToLower() == publisherName.ToLower()).FirstOrDefault()!;
        }

        public string GetPublisherMiniPicturePath(int publisherId)
        {
            return dataContext.Publishers.Where(p => p.Id == publisherId).
                Select(p => p.MiniPicturePath).FirstOrDefault()!;
        }

        public string GetPublisherPicturePath(int publisherId)
        {
            return dataContext.Publishers.Where(p => p.Id == publisherId).
                Select(p => p.PicturePath).FirstOrDefault()!;
        }
        
        public IList<Publisher> GetPublishers()
        {
            return dataContext.Publishers.OrderBy(p => p.Id).ToList();
        }

        public bool PublisherExists(int publisherId)
        {
            return dataContext.Publishers.Any(p => p.Id == publisherId);
        }
    }
}
