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

        public IList<PersonGame> PersonGames(int libraryId)
        {
            return dataContext.PersonGames.Include(g => g.Game).ThenInclude(r => r.Rating).Include(g => g.Game).
                ThenInclude(r => r.Developers).Include(g => g.Game).ThenInclude(r => r.Publishers).Include(g => g.Game).
                ThenInclude(r => r.Platforms).Include(g => g.Game).ThenInclude(r => r.Genres).Include(g => g.Game).
                ThenInclude(r => r.Tags).Where(pg => pg.Library.Id == libraryId).ToList();
        }

        public IList<PersonGame> PersonGamesByList(int libraryId, string list)
        {
            return dataContext.PersonGames.Include(g => g.Game).ThenInclude(r => r.Rating).Include(g => g.Game).
                ThenInclude(r => r.Developers).Include(g => g.Game).ThenInclude(r => r.Publishers).Include(g => g.Game).
                ThenInclude(r => r.Platforms).Include(g => g.Game).ThenInclude(r => r.Genres).Include(g => g.Game).
                ThenInclude(r => r.Tags).Where(pg => pg.Library.Id == libraryId && pg.List.ToLower() == list.ToLower()).
                OrderBy(pg => pg.Score).ToList();
        }

        public bool PersonLibraryExists(int libraryId)
        {
            return dataContext.Libraries.Any(l => l.Id == libraryId);
        }
    }
}
