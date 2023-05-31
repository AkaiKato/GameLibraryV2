using GameLibraryV2.Dto.Statistic;
using GameLibraryV2.Models.Common;

namespace GameLibraryV2.Interfaces
{
    public interface IPersonGamesRepository
    {
        public Task<IList<PersonGame>> GetAllPersonGamesAsync();

        public Task<IList<PersonGame>> PersonGamesAsync(int userId);

        public Task<IList<PersonGame>> PersonGamesByListAsync(int userId, string list);

        public Task<PersonGame> GetPersonGameByIdAsync(Guid personGameId);

        public Task<PersonGame> GetPersonGameByUserIdAndGameIdAsync(int userId, int gameId);

        public Task<IList<CountStatistic>> GetPersonPublisherStatisticAsync(int userId);

        public Task<IList<CountStatistic>> GetPersonTagStatisticAsync(int userId);

        public Task<IList<CountStatistic>> GetPersonDeveloperStatisticAsync(int userId);

        public Task<IList<CountStatistic>> GetPersonPlatformStatisticAsync(int userId);

        public Task<IList<CountStatistic>> GetPersonGenreStatisticAsync(int userId);

        public Task<IList<Game>> GetPersonFavouriteGameAsync(int userId);

        public Task<bool> PersonGameExistsAsync(Guid personGameId);

        public Task<bool> PersonGameExistsAsync(int userId, int gameId);

        public void CreatePersonGame(PersonGame game);

        public void UpdatePersonGame(PersonGame game);

        public void DeletePersonGame(PersonGame game);

        public Task SavePersonGameAsync();
    }
}
