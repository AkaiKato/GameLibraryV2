using GameLibraryV2.Dto.Statistic;
using GameLibraryV2.Models.Common;

namespace GameLibraryV2.Interfaces
{
    public interface IPersonGamesRepository
    {
        public IList<PersonGame> GetAllPersonGames();

        public IList<PersonGame> PersonGames(int userId);

        public IList<PersonGame> PersonGamesByList(int userId, string list);

        public PersonGame GetPersonGameById(Guid personGameId);

        public PersonGame GetPersonGameByUserIdAndGameId(int userId, int gameId);

        public IList<CountStatistic> GetPersonPublisherStatistic(int userId);

        public IList<CountStatistic> GetPersonTagStatistic(int userId);

        public IList<CountStatistic> GetPersonDeveloperStatistic(int userId);

        public IList<CountStatistic> GetPersonPlatformStatistic(int userId);

        public IList<CountStatistic> GetPersonGenreStatistic(int userId);

        public IList<Game> GetPersonFavouriteGame(int userId);

        public bool PersonGameExists(Guid personGameId);

        public bool PersonGameExists(int userId, int gameId);

        public bool CreatePersonGame(PersonGame game);

        public bool UpdatePersonGame(PersonGame game);

        public bool DeletePersonGame(PersonGame game);
    }
}
