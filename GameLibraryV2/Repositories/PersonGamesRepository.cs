using GameLibraryV2.Data;
using GameLibraryV2.Dto.Statistic;
using GameLibraryV2.Interfaces;
using GameLibraryV2.Models.Common;
using Microsoft.EntityFrameworkCore;

namespace GameLibraryV2.Repositories
{
    public class PersonGamesRepository : IPersonGamesRepository
    {
        private readonly DataContext dataContext;

        public PersonGamesRepository(DataContext context)
        {
            dataContext = context;
        }

        public IList<PersonGame> GetAllPersonGames()
        {
            return dataContext.PersonGames.Include(g => g.Game).ThenInclude(r => r.Rating).Include(g => g.Game).
                ThenInclude(r => r.Developers).Include(g => g.Game).ThenInclude(r => r.Publishers).Include(g => g.Game).
                ThenInclude(r => r.Platforms).Include(g => g.Game).ThenInclude(r => r.Genres).Include(g => g.Game).
                ThenInclude(r => r.Tags).Include(pl => pl.PlayedPlatform).ToList();
        }

        public IList<PersonGame> PersonGames(int userId)
        {
            return dataContext.PersonGames.Include(g => g.Game).ThenInclude(r => r.Rating).Include(g => g.Game).
                ThenInclude(r => r.Developers).Include(g => g.Game).ThenInclude(r => r.Publishers).Include(g => g.Game).
                ThenInclude(r => r.Platforms).Include(g => g.Game).ThenInclude(r => r.Genres).Include(g => g.Game).
                ThenInclude(r => r.Tags).Include(pl => pl.PlayedPlatform).Where(pg => pg.User.Id == userId).ToList();
        }

        public IList<PersonGame> PersonGamesByList(int userId, string list)
        {
            return dataContext.PersonGames.Include(g => g.Game).ThenInclude(r => r.Rating).Include(g => g.Game).
                ThenInclude(r => r.Developers).Include(g => g.Game).ThenInclude(r => r.Publishers).Include(g => g.Game).
                ThenInclude(r => r.Platforms).Include(g => g.Game).ThenInclude(r => r.Genres).Include(g => g.Game).
                ThenInclude(r => r.Tags).Where(pg => pg.User.Id == userId && pg.List.ToLower() == list.ToLower()).
                OrderBy(pg => pg.Score).ToList();
        }

        public PersonGame GetPersonGameById(Guid personGameId)
        {
            return dataContext.PersonGames.Include(u => u.User).Include(g => g.Game).ThenInclude(r => r.Rating).Where(pg => pg.Id == personGameId).FirstOrDefault()!;
        }

        public PersonGame GetPersonGameByUserIdAndGameId(int userId, int gameId)
        {
            return dataContext.PersonGames.Where(u => u.User.Id == userId && u.Game.Id == gameId).FirstOrDefault()!;
        }

        public IList<CountStatistic> GetPersonPublisherStatistic(int userId)
        {
            return dataContext.PersonGames.Where(pg => pg.User.Id == userId)
                .SelectMany(x => x.Game.Publishers)
                .GroupBy(x => new {x.Id, x.Name})
                .Select(x => new CountStatistic{ Id = x.Key.Id, Name = x.Key.Name, Count = x.Count()})
                .OrderByDescending(x => x.Count)
                .ToList();
        }

        public IList<CountStatistic> GetPersonTagStatistic(int userId)
        {
            return dataContext.PersonGames.Where(pg => pg.User.Id == userId)
                .SelectMany(x => x.Game.Tags)
                .GroupBy(x => new { x.Id, x.Name })
                .Select(x => new CountStatistic { Id = x.Key.Id, Name = x.Key.Name, Count = x.Count()})
                .OrderByDescending(x => x.Count)
                .ToList();
        }

        public IList<CountStatistic> GetPersonDeveloperStatistic(int userId)
        {
            return dataContext.PersonGames.Where(pg => pg.User.Id == userId)
                .SelectMany(x => x.Game.Developers)
                .GroupBy(x => new { x.Id, x.Name })
                .Select(x => new CountStatistic { Id = x.Key.Id, Name = x.Key.Name, Count = x.Count() })
                .OrderByDescending(x => x.Count)
                .ToList();
        }

        public IList<CountStatistic> GetPersonPlatformStatistic(int userId)
        {
            return dataContext.PersonGames.Where(pg => pg.User.Id == userId && pg.PlayedPlatform != null)
                .Select(x => x.PlayedPlatform)
                .GroupBy(x => new { x!.Id, x.Name })
                .Select(x => new CountStatistic { Id = x.Key.Id, Name = x.Key.Name, Count = x.Count() })
                .OrderByDescending(x => x.Count)
                .ToList();
        }

        public IList<CountStatistic> GetPersonGenreStatistic(int userId)
        {
            return dataContext.PersonGames.Where(pg => pg.User.Id == userId)
                .SelectMany(x => x.Game.Genres)
                .GroupBy(x => new { x.Id, x.Name })
                .Select(x => new CountStatistic { Id = x.Key.Id, Name = x.Key.Name, Count = x.Count() })
                .OrderByDescending(x => x.Count)
                .ToList();
        }

        public IList<Game> GetPersonFavouriteGame(int userId)
        {
            return dataContext.PersonGames.Where(pg => pg.User.Id == userId && pg.Favourite == true)
                .Include(g => g.Game).ThenInclude(r => r.Rating).Include(g => g.Game).
                ThenInclude(r => r.Developers).Include(g => g.Game).ThenInclude(r => r.Publishers).Include(g => g.Game).
                ThenInclude(r => r.Platforms).Include(g => g.Game).ThenInclude(r => r.Genres).Include(g => g.Game).
                ThenInclude(r => r.Tags)
                .Select(x => x.Game).ToList();
        }

        public bool PersonGameExists(Guid personGameId)
        {
            return dataContext.PersonGames.Any(pg => pg.Id == personGameId);
        }

        public bool PersonGameExists(int userId, int gameId)
        {
            return dataContext.PersonGames.Any(u => u.User.Id == userId && u.Game.Id == gameId);
        }

        public bool CreatePersonGame(PersonGame personGame)
        {
            dataContext.Add(personGame);
            return Save();
        }

        public bool UpdatePersonGame(PersonGame personGame)
        {
            dataContext.Update(personGame);
            return Save();
        }

        public bool DeletePersonGame(PersonGame personGame)
        {
            dataContext.Remove(personGame);
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
