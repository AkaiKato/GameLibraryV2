using GameLibraryV2.Models;

namespace GameLibraryV2.Interfaces
{
    public interface IGenreRepository
    {
        public Genre GetGenreById(int genreId);

        public Genre GetGenreByName(string genreName);

        public IList<Genre> GetGenres();

        public bool GenreExists(int genreId);

        public bool CreateGenre(Genre genre);
    }
}
