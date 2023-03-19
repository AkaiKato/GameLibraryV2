using GameLibraryV2.Models;

namespace GameLibraryV2.Interfaces
{
    public interface IPersonGamesRepository
    {
        public IList<PersonGame> PersonGames(int libraryId);

        public IList<PersonGame> PersonGamesByList(int libraryId, string list);

        public bool PersonLibraryExists(int libraryId);
    }
}
