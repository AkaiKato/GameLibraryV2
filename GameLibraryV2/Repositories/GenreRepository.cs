using GameLibraryV2.Data;
using GameLibraryV2.Interfaces;
using GameLibraryV2.Models.Common;

namespace GameLibraryV2.Repositories
{
    public class GenreRepository : IGenreRepository
    {
        private readonly DataContext dataContext;

        public GenreRepository(DataContext context)
        {
            dataContext = context;
        }

        public bool GenreExists(int genreId)
        {
            return dataContext.Genres.Any(g => g.Id == genreId);
        }

        public bool GenreNameAlredyInUse(int genreId, string genreName)
        {
            return dataContext.Genres.Any(g => g.Name.Trim().ToLower() == genreName.Trim().ToLower() && g.Id != genreId);
        }

        public Genre GetGenreById(int genreId)
        {
            return dataContext.Genres.Where(g => g.Id == genreId).FirstOrDefault()!;
        }

        public Genre GetGenreByName(string genreName)
        {
            return dataContext.Genres.Where(g => g.Name.Trim().ToLower() == genreName.Trim().ToLower()).FirstOrDefault()!;
        }

        public IList<Genre> GetGenres()
        {
            return dataContext.Genres.OrderBy(g => g.Id).ToList();
        }

        //--------------------------------------------

        public bool CreateGenre(Genre genre)
        {
            dataContext.Add(genre);
            return Save();
        }

        public bool UpdateGenre(Genre genre)
        {
            dataContext.Update(genre);
            return Save();
        }

        public bool DeleteGenre(Genre genre)
        {
            dataContext.Remove(genre);
            return Save();
        }

        private bool Save()
        {
            var saved = dataContext.SaveChanges();
            //var saved = 1;
            return saved > 0;
        }
    }
}
