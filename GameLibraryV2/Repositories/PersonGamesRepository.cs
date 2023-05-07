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

        public async Task<IList<PersonGame>> GetAllPersonGamesAsync()
        {
            return await dataContext.PersonGames
                .Include(g => g.Game).ThenInclude(r => r.Rating)
                .Include(g => g.Game).ThenInclude(r => r.Developers)
                .Include(g => g.Game).ThenInclude(r => r.Publishers)
                .Include(g => g.Game).ThenInclude(r => r.Platforms)
                .Include(g => g.Game).ThenInclude(r => r.Genres)
                .Include(g => g.Game).ThenInclude(r => r.Tags)
                .Include(pl => pl.PlayedPlatform)
                .ToListAsync();
        }

        public async Task<IList<PersonGame>> PersonGamesAsync(int userId)
        {
            return await dataContext.PersonGames
                .Include(g => g.Game).ThenInclude(r => r.Rating)
                .Include(g => g.Game).ThenInclude(r => r.Developers)
                .Include(g => g.Game).ThenInclude(r => r.Publishers)
                .Include(g => g.Game).ThenInclude(r => r.Platforms)
                .Include(g => g.Game).ThenInclude(r => r.Genres)
                .Include(g => g.Game).ThenInclude(r => r.Tags)
                .Include(pl => pl.PlayedPlatform)
                .Where(pg => pg.User.Id == userId)
                .ToListAsync();
        }

        public async Task<IList<PersonGame>> PersonGamesByListAsync(int userId, string list)
        {
            return await dataContext.PersonGames
                .Include(g => g.Game).ThenInclude(r => r.Rating)
                .Include(g => g.Game).ThenInclude(r => r.Developers)
                .Include(g => g.Game).ThenInclude(r => r.Publishers)
                .Include(g => g.Game).ThenInclude(r => r.Platforms)
                .Include(g => g.Game).ThenInclude(r => r.Genres)
                .Include(g => g.Game).ThenInclude(r => r.Tags)
                .Where(pg => pg.User.Id == userId && pg.List.ToLower() == list.ToLower())
                .OrderBy(pg => pg.Score)
                .ToListAsync();
        }

        public async Task<PersonGame> GetPersonGameByIdAsync(Guid personGameId)
        {
            return await dataContext.PersonGames.Include(u => u.User)
                .Include(g => g.Game).ThenInclude(r => r.Rating)
                .FirstOrDefaultAsync(pg => pg.Id == personGameId)!;
        }

        public async Task<PersonGame> GetPersonGameByUserIdAndGameIdAsync(int userId, int gameId)
        {
            return await dataContext.PersonGames
                .FirstOrDefaultAsync(u => u.User.Id == userId && u.Game.Id == gameId)!;
        }

        public async Task<IList<CountStatistic>> GetPersonPublisherStatisticAsync(int userId)
        {
            return await dataContext.PersonGames.Where(pg => pg.User.Id == userId)
                .SelectMany(x => x.Game.Publishers)
                .GroupBy(x => new {x.Id, x.Name})
                .Select(x => new CountStatistic{ Id = x.Key.Id, Name = x.Key.Name, Count = x.Count()})
                .OrderByDescending(x => x.Count)
                .ToListAsync();
        }

        public async Task<IList<CountStatistic>> GetPersonTagStatisticAsync(int userId)
        {
            return await dataContext.PersonGames.Where(pg => pg.User.Id == userId)
                .SelectMany(x => x.Game.Tags)
                .GroupBy(x => new { x.Id, x.Name })
                .Select(x => new CountStatistic { Id = x.Key.Id, Name = x.Key.Name, Count = x.Count()})
                .OrderByDescending(x => x.Count)
                .ToListAsync();
        }

        public async Task<IList<CountStatistic>> GetPersonDeveloperStatisticAsync(int userId)
        {
            return await dataContext.PersonGames.Where(pg => pg.User.Id == userId)
                .SelectMany(x => x.Game.Developers)
                .GroupBy(x => new { x.Id, x.Name })
                .Select(x => new CountStatistic { Id = x.Key.Id, Name = x.Key.Name, Count = x.Count() })
                .OrderByDescending(x => x.Count)
                .ToListAsync();
        }

        public async Task<IList<CountStatistic>> GetPersonPlatformStatisticAsync(int userId)
        {
            return await dataContext.PersonGames.Where(pg => pg.User.Id == userId && pg.PlayedPlatform != null)
                .Select(x => x.PlayedPlatform)
                .GroupBy(x => new { x!.Id, x.Name })
                .Select(x => new CountStatistic { Id = x.Key.Id, Name = x.Key.Name, Count = x.Count() })
                .OrderByDescending(x => x.Count)
                .ToListAsync();
        }

        public async Task<IList<CountStatistic>> GetPersonGenreStatisticAsync(int userId)
        {
            return await dataContext.PersonGames.Where(pg => pg.User.Id == userId)
                .SelectMany(x => x.Game.Genres)
                .GroupBy(x => new { x.Id, x.Name })
                .Select(x => new CountStatistic { Id = x.Key.Id, Name = x.Key.Name, Count = x.Count() })
                .OrderByDescending(x => x.Count)
                .ToListAsync();
        }

        public async Task<IList<Game>> GetPersonFavouriteGameAsync(int userId)
        {
            return await dataContext.PersonGames
                .Where(pg => pg.User.Id == userId && pg.Favourite == true)
                .Include(g => g.Game).ThenInclude(r => r.Rating)
                .Include(g => g.Game).ThenInclude(r => r.Developers)
                .Include(g => g.Game).ThenInclude(r => r.Publishers)
                .Include(g => g.Game).ThenInclude(r => r.Platforms)
                .Include(g => g.Game).ThenInclude(r => r.Genres)
                .Include(g => g.Game).ThenInclude(r => r.Tags)
                .Select(x => x.Game).ToListAsync();
        }

        public async Task<bool> PersonGameExistsAsync(Guid personGameId)
        {
            return await dataContext.PersonGames.AnyAsync(pg => pg.Id == personGameId);
        }

        public async Task<bool> PersonGameExistsAsync(int userId, int gameId)
        {
            return await dataContext.PersonGames.AnyAsync(u => u.User.Id == userId && u.Game.Id == gameId);
        }

        public void CreatePersonGame(PersonGame personGame)
        {
            dataContext.Add(personGame);
        }

        public void UpdatePersonGame(PersonGame personGame)
        {
            dataContext.Update(personGame);
        }

        public void DeletePersonGame(PersonGame personGame)
        {
            dataContext.Remove(personGame);
        }

        public async Task SavePersonGameAsync()
        {
            await dataContext.SaveChangesAsync();
        }
    }
}
