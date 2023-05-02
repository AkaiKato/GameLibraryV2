using GameLibraryV2.Models.Common;

namespace GameLibraryV2.Interfaces
{
    public interface IGenreRepository
    {
        public Genre GetGenreById(int genreId);

        public Genre GetGenreByName(string genreName);

        public IList<Genre> GetGenres();

        public bool GenreExists(int genreId);

        public bool GenreNameAlredyInUse(int genreId, string genreName);

        public bool CreateGenre(Genre genre);

        public bool UpdateGenre(Genre genre);

        public bool DeleteGenre(Genre genre);
    }
}
