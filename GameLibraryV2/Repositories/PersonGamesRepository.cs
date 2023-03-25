using GameLibraryV2.Data;
using GameLibraryV2.Interfaces;
using GameLibraryV2.Models;
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

        public IList<PersonGame> PersonGames(int userId)
        {
            return dataContext.PersonGames.Include(g => g.Game).ThenInclude(r => r.Rating).Include(g => g.Game).
                ThenInclude(r => r.Developers).Include(g => g.Game).ThenInclude(r => r.Publishers).Include(g => g.Game).
                ThenInclude(r => r.Platforms).Include(g => g.Game).ThenInclude(r => r.Genres).Include(g => g.Game).
                ThenInclude(r => r.Tags).Where(pg => pg.User.Id == userId).ToList();
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
            return dataContext.PersonGames.Where(pg => pg.Id == personGameId).FirstOrDefault()!;
        }

        public bool PersonGameExists(Guid personGameId)
        {
            return dataContext.PersonGames.Any(pg => pg.Id == personGameId);
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
