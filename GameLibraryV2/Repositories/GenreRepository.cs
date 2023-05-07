using GameLibraryV2.Data;
using GameLibraryV2.Interfaces;
using GameLibraryV2.Models.Common;
using Microsoft.EntityFrameworkCore;

namespace GameLibraryV2.Repositories
{
    public class GenreRepository : IGenreRepository
    {
        private readonly DataContext dataContext;

        public GenreRepository(DataContext context)
        {
            dataContext = context;
        }

        public async Task<bool> GenreExistsAsync(int genreId)
        {
            return await dataContext.Genres.AnyAsync(g => g.Id == genreId);
        }

        public async Task<bool> GenreNameAlredyInUseAsync(int genreId, string genreName)
        {
            return await dataContext.Genres.AnyAsync(g => g.Name.Trim().ToLower() == genreName.Trim().ToLower() && g.Id != genreId);
        }

        public async Task<Genre> GetGenreByIdAsync(int genreId)
        {
            return await dataContext.Genres.FirstOrDefaultAsync(g => g.Id == genreId);
        }

        public async Task<Genre> GetGenreByNameAsync(string genreName)
        {
            return await dataContext.Genres.FirstOrDefaultAsync(g => g.Name.Trim().ToLower() == genreName.Trim().ToLower());
        }

        public async Task<IList<Genre>> GetGenresAsync()
        {
            return await dataContext.Genres.OrderBy(g => g.Id).ToListAsync();
        }

        //--------------------------------------------

        public void CreateGenre(Genre genre)
        {
            dataContext.Add(genre);
        }

        public void UpdateGenre(Genre genre)
        {
            dataContext.Update(genre);
        }

        public void DeleteGenre(Genre genre)
        {
            dataContext.Remove(genre);
        }

        public async Task SaveGenreAsync()
        {
            await dataContext.SaveChangesAsync();
        }
    }
}
