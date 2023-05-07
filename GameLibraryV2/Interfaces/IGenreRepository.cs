using GameLibraryV2.Models.Common;

namespace GameLibraryV2.Interfaces
{
    public interface IGenreRepository
    {
        public Task<Genre> GetGenreByIdAsync(int genreId);

        public Task<Genre> GetGenreByNameAsync(string genreName);

        public Task<IList<Genre>> GetGenresAsync();

        public Task<bool> GenreExistsAsync(int genreId);

        public Task<bool> GenreNameAlredyInUseAsync(int genreId, string genreName);

        public void CreateGenre(Genre genre);

        public void UpdateGenre(Genre genre);

        public void DeleteGenre(Genre genre);

        public Task SaveGenreAsync();
    }
}
