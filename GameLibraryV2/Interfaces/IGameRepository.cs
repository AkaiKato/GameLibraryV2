using GameLibraryV2.Helper;
using GameLibraryV2.Models;

namespace GameLibraryV2.Interfaces
{
    public interface IGameRepository
    {
        public PagedList<Game> GetGames(SearchParameters searchParameters);

        public Game GetGameById(int gameId);

        public Game GetGameByName(string gameName);

        public Game GetDLCById(int dlcId);

        public Game GetDLCByName(string dlcName);

        public IList<Game> GetDLCs();

        public IList<Game> GetGamesByAgeRating(int ageratingId);

        public IList<Game> GetGamesByDeveloper(int developerId);

        public IList<Game> GetGamesByGenre(int genreId);

        public IList<Game> GetGameByPlatform(int platformId);

        public IList<Game> GetGamesByPublisher(int publisherId);

        public IList<Game> GetGamesByTag(int tagId);

        public bool GameExists(int gameId);

        public bool GameNameAlreadyInUse(int gameId, string gameName);

        public bool DLCExists(int dlcId);

        public bool CreateGame(Game game);

        public bool UpdateGame(Game game);

        public bool DeleteGame(Game game);
    }
}
