using GameLibraryV2.Helper;
using GameLibraryV2.Models;
using GameLibraryV2.Models.Common;

namespace GameLibraryV2.Interfaces
{
    public interface IGameRepository
    {
        /// <summary>
        /// Фильтрует по полученным данным.
        /// </summary>
        /// <param name="filterParameters"></param>
        /// <returns></returns>
        public PagedList<Game> GetGames(FilterParameters filterParameters);

        public Game GetGameById(int gameId);

        public Game GetGameByName(string gameName);

        public Game GetDLCById(int dlcId);

        public Game GetDLCByName(string dlcName);

        public IList<Game> GetDLCs();

        public PagedList<Game> GetGamesByAgeRating(int ageratingId, FilterParameters filterParameters);

        public PagedList<Game> GetGamesByDeveloper(int developerId, FilterParameters filterParameters);

        public PagedList<Game> GetGamesByGenre(int genreId, FilterParameters filterParameters);

        public PagedList<Game> GetGameByPlatform(int platformId, FilterParameters filterParameters);

        public PagedList<Game> GetGamesByPublisher(int publisherId, FilterParameters filterParameters);

        public PagedList<Game> GetGamesByTag(int tagId, FilterParameters filterParameters);

        public bool GameExists(int gameId);

        public bool GameNameAlreadyInUse(int gameId, string gameName);

        public bool DLCExists(int dlcId);

        public bool CreateGame(Game game);

        public bool UpdateGame(Game game);

        public bool DeleteGame(Game game);
    }
}
