using GameLibraryV2.Models;

namespace GameLibraryV2.Interfaces
{
    public interface IGameRepository
    {
        public ICollection<Game> GetGames();

        public Game GetGameById(int gameId);

        public Game GetGameByName(string gameName);

        public IList<Review> GetGameReviews(int gameId);

        public string GetGamePicturePath(int gameId);

        public bool GameExists(int gameId);
    }
}
