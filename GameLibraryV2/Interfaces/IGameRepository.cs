using GameLibraryV2.Helper;
using GameLibraryV2.Models;
using GameLibraryV2.Models.Common;

namespace GameLibraryV2.Interfaces
{
    public interface IGameRepository
    {
        public PagedList<Game> GetGamesOrderByRating(FilterParameters filterParameters);

        public PagedList<Game> GetGamesOrderByName(FilterParameters filterParameters);

        public Game GetGameById(int gameId);

        public Game GetGameByName(string gameName);

        public Game GetDLCById(int dlcId);

        public Game GetDLCByName(string dlcName);

        public IList<Game> GetDLCs();

        public PagedList<Game> GetGamesByAgeRatingOrderByRating(int ageratingId, FilterParameters filterParameters);

        public PagedList<Game> GetGamesByAgeRatingOrderByName(int ageratingId, FilterParameters filterParameters);

        public PagedList<Game> GetGamesByDeveloperOrderByRating(int developerId, FilterParameters filterParameters);

        public PagedList<Game> GetGamesByDeveloperOrderByName(int developerId, FilterParameters filterParameters);

        public PagedList<Game> GetGamesByGenreOrderByRating(int genreId, FilterParameters filterParameters);

        public PagedList<Game> GetGamesByGenreOrderByName(int genreId, FilterParameters filterParameters);

        public PagedList<Game> GetGameByPlatformOrderByRating(int platformId, FilterParameters filterParameters);

        public PagedList<Game> GetGameByPlatformOrderByName(int platformId, FilterParameters filterParameters);

        public PagedList<Game> GetGamesByPublisherOrderByRating(int publisherId, FilterParameters filterParameters);

        public PagedList<Game> GetGamesByPublisherOrderByName(int publisherId, FilterParameters filterParameters);

        public PagedList<Game> GetGamesByTagOrderByRating(int tagId, FilterParameters filterParameters);

        public PagedList<Game> GetGamesByTagOrderByName(int tagId, FilterParameters filterParameters);

        public bool GameExists(int gameId);

        public bool GameNameAlreadyInUse(int gameId, string gameName);

        public bool DLCExists(int dlcId);

        public bool CreateGame(Game game);

        public bool UpdateGame(Game game);

        public bool DeleteGame(Game game);
    }
}
