using GameLibraryV2.Data;
using GameLibraryV2.Helper;
using GameLibraryV2.Models;
using GameLibraryV2.Models.Common;

namespace GameLibraryV2.Interfaces
{
    public interface IGameRepository
    {
        public Task<PagedList<Game>> GetGamesAsync(FilterParameters filterParameters);

        public Task<IList<Game>> GetGamesThatContainsStringAsync(string searchString);

        public Task<Game> GetGameByIdAsync(int gameId);

        public Task<Game> GetGameByNameAsync(string gameName);

        public Task<Game> GetDLCByIdAsync(int dlcId);

        public Task<Game> GetDLCByNameAsync(string dlcName);

        public Task<IList<Game>> GetDLCsAsync();

        public Task<PagedList<Game>> GetGamesByAgeRatingAsync(int ageratingId, FilterParameters filterParameters);

        public Task<PagedList<Game>> GetGamesByDeveloperAsync(int developerId, FilterParameters filterParameters);

        public Task<PagedList<Game>> GetGamesByGenreAsync(int genreId, FilterParameters filterParameters);

        public Task<PagedList<Game>> GetGameByPlatformAsync(int platformId, FilterParameters filterParameters);

        public Task<PagedList<Game>> GetGamesByPublisherAsync(int publisherId, FilterParameters filterParameters);

        public Task<PagedList<Game>> GetGamesByTagAsync(int tagId, FilterParameters filterParameters);

        public Task<bool> GameExistsAsync(int gameId);

        public Task<bool> GameNameAlreadyInUseAsync(int gameId, string gameName);

        public Task<bool> DLCExistsAsync(int dlcId);

        public Task<int> GetTotalNumberOfGames();

        public Task<int> GetNumberOfGames();

        public Task<int> GetNumberOfDLC();

        public Task<Game> GetMostRatedGame();

        public Task<Game> GetMostRatedDLC();

        public Task<List<Game>> GetMostRatedGameByYear();

        public Task<List<Game>> GetMostRatedDLCByYear();

        public Task<DateOnly> GetMinReleaseDate();

        public Task<DateOnly> GetMaxReleaseDate();

        public void CreateGame(Game game);

        public void UpdateGame(Game game);

        public void DeleteGame(Game game);

        public Task SaveGameAsync();
    }
}
