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
                Include(s => s.SystemRequirementsMax).Include(r => r.Rating).Where(x => x.Name.Trim().ToLower() == gameName.Trim().ToLower()).FirstOrDefault()!;
        }

        public IList<Review> GetGameReviews(int gameId)
        {
            return dataContext.Reviews.Include(u => u.User).Where(d => d.Game.Id == gameId).ToList();
        }

        public string GetGamePicturePath(int gameId)
        {
            return dataContext.Games.Where(g => g.Id == gameId).Select(pp => pp.PicturePath).FirstOrDefault()!;
        }

        public bool CreateGame(int[] DeveloperIds, int[] PublisherIds, int[] PlatformIds, int[] GenreIds, int[] TagIds, Game game)
        {
            var devS = new List<Developer>();
            foreach (var item in DeveloperIds)
                devS.Add(dataContext.Developers.Where(d => d.Id == item).FirstOrDefault()!);

            var pubS = new List<Publisher>();
            foreach (var item in PublisherIds)
                pubS.Add(dataContext.Publishers.Where(d => d.Id == item).FirstOrDefault()!);

            var platS = new List<Platform>();
            foreach (var item in PlatformIds)
                platS.Add(dataContext.Platforms.Where(d => d.Id == item).FirstOrDefault()!);

            var genrS = new List<Genre>();
            foreach (var item in GenreIds)
                genrS.Add(dataContext.Genres.Where(d => d.Id == item).FirstOrDefault()!);

            var tagS = new List<Tag>();
            foreach (var item in TagIds)
                tagS.Add(dataContext.Tags.Where(d => d.Id == item).FirstOrDefault()!);

            game.Developers = devS;
            game.Publishers = pubS;
            game.Platforms = platS;
            game.Genres = genrS;
            game.Tags = tagS;

            dataContext.Add(game);
            return Save();
        }

        public bool SaveGamePicturePath(int gameId, string path)
        {
            var game = dataContext.Games.Where(g => g.Id == gameId).FirstOrDefault();
            game!.PicturePath = path;
            dataContext.Games.Update(game);
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
