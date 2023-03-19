using GameLibraryV2.Data;
using GameLibraryV2.Interfaces;
using GameLibraryV2.Models;
using Microsoft.EntityFrameworkCore;

namespace GameLibraryV2.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly DataContext dataContext;
     
        public GameRepository(DataContext context)
        {
            dataContext = context;
        }

        public ICollection<Game> GetGames()
        {
            return dataContext.Games.Select(p => new Game
            {
                Id = p.Id,
                Name = p.Name,
                PicturePath = p.PicturePath,
                ReleaseDate = p.ReleaseDate,
                Description = p.Description,
                AgeRating = p.AgeRating,
                NSFW = p.NSFW,
                Type = p.Type,
                AveragePlayTime = p.AveragePlayTime,
                Rating = p.Rating,
                Developers = p.Developers.Select(t => new Developer
                {
                    Id = t.Id,
                    Name = t.Name,
                }).ToList(),
                Publishers = p.Publishers.Select(t => new Publisher 
                { 
                    Id = t.Id,
                    Name = t.Name,
                }).ToList(),
                Platforms = p.Platforms.Select(t => new Platform
                { 
                    Id = t.Id,
                    Name = t.Name,
                }).ToList(),
                Genres = p.Genres.Select(t => new Genre
                { 
                    Id = t.Id,
                    Name = t.Name,
                }).ToList(),
                Tags = p.Tags.Select(t => new Tag 
                { 
                    Id = t.Id,
                    Name = t.Name,
                }).ToList(),

            }).ToList();
        }

        public bool GameExists(int gameId)
        {
            return dataContext.Games.Any(x => x.Id == gameId);
        }

        public Game GetGameById(int gameId)
        {
            return dataContext.Games.Include(d => d.DLCs)!.ThenInclude(dg => dg.DLCGame).Include(d => d.Developers).
                Include(p => p.Publishers).Include(p => p.Platforms).Include(g => g.Genres).
                Include(t => t.Tags).Include(p => p.ParentGame).Include(s => s.SystemRequirementsMin).
                Include(s => s.SystemRequirementsMax).Include(r => r.Rating).Where(x => x.Id == gameId).FirstOrDefault()!;
        }

        public Game GetGameByName(string gameName)
        {
            return dataContext.Games.Include(d => d.DLCs)!.ThenInclude(dg => dg.DLCGame).Include(d => d.Developers).
                Include(p => p.Publishers).Include(p => p.Platforms).Include(g => g.Genres).
                Include(t => t.Tags).Include(p => p.ParentGame).Include(s => s.SystemRequirementsMin).
                Include(s => s.SystemRequirementsMax).Include(r => r.Rating).Where(x => x.Name == gameName).FirstOrDefault()!;
        }

        public IList<Review> GetGameReviews(int gameId)
        {
            return dataContext.Reviews.Include(u => u.User).Where(d => d.Game.Id == gameId).ToList();
        }

        public string GetGamePicturePath(int gameId)
        {
            return dataContext.Games.Where(g => g.Id == gameId).Select(pp => pp.PicturePath).FirstOrDefault()!;
        }
    }
}
