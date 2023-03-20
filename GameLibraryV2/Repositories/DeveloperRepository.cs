using GameLibraryV2.Data;
using GameLibraryV2.Interfaces;
using GameLibraryV2.Models;
using Microsoft.EntityFrameworkCore;

namespace GameLibraryV2.Repositories
{
    public class DeveloperRepository : IDeveloperRepository
    {
        private readonly DataContext dataContext;

        public DeveloperRepository(DataContext context)
        {
            dataContext = context;
        }

        public bool DeveloperExists(int developerId)
        {
            return dataContext.Developers.Any(d => d.Id == developerId);
        }

        public Developer GetDeveloperById(int developerId)
        {
            return dataContext.Developers.FirstOrDefault(d => d.Id == developerId)!;    
        }

        public Developer GetDeveloperByName(string developerName)
        {
            return dataContext.Developers.FirstOrDefault(d => d.Name.Trim().ToLower() == developerName.Trim().ToLower())!;
        }

        public string GetDeveloperMiniPicturePath(int developerId)
        {
            return dataContext.Developers.Where(d => d.Id == developerId).Select(d => d.MiniPicturePath).FirstOrDefault()!;
        }

        public string GetDeveloperPicturePath(int developerId)
        {
            return dataContext.Developers.Where(d => d.Id == developerId).Select(d => d.MiniPicturePath).FirstOrDefault()!;
        }

        public IList<Developer> GetDevelopers()
        {
            return dataContext.Developers.OrderBy(d => d.Id).ToList();
        }

        public IList<Game> GetGamesByDeveloper(int developerId)
        {
            return dataContext.Games.Where(g => g.Developers.Any(d => d.Id == developerId)).Select(g => new Game
            {
                Id = g.Id,
                Name = g.Name,
                PicturePath= g.PicturePath,
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

        //------------------------------------------------------------------

        public bool CreateDeveloper(Developer developer)
        {
            dataContext.Add(developer);
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
