using GameLibraryV2.Models;

namespace GameLibraryV2.Interfaces
{
    public interface IPersonGamesRepository
    {
        public IList<PersonGame> GetAllPersonGames();

        public IList<PersonGame> PersonGames(int userId);

        public IList<PersonGame> PersonGamesByList(int userId, string list);

        public PersonGame GetPersonGameById(Guid personGameId);

        public PersonGame GetPersonGameByUserIdAndGameId(int userId, int gameId);

        public bool PersonGameExists(Guid personGameId);

        public bool PersonGameExists(int userId, int gameId);

        public bool CreatePersonGame(PersonGame game);

        public bool UpdatePersonGame(PersonGame game);

        public bool DeletePersonGame(PersonGame game);
    }
}
