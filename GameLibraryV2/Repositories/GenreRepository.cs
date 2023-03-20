using GameLibraryV2.Data;
using GameLibraryV2.Interfaces;
using GameLibraryV2.Models;

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

        public IList<Game> GetGamesByGenre(int genreId)
        {
            return dataContext.Games.Where(g => g.Genres.Any(g => g.Id == genreId)).Select(g => new Game
            {
                Id = g.Id,
                Name = g.Name,
                PicturePath= g.PicturePath,
                ReleaseDate = g.ReleaseDate,
                Description = g.Description,
                AgeRating = g.AgeRating,
                NSFW = g.NSFW,
                Type = g.Type,
                AveragePlayTime = g.AveragePlayTime,
                Rating = g.Rating,
                Developers = g.Developers.Select(t => new Developer
                {
                    Id = t.Id,
                    Name = t.Name,
                }).ToList(),
                Publishers = g.Publishers.Select(t => new Publisher
                {
                    Id = t.Id,
                    Name = t.Name,
                }).ToList(),
                Platforms = g.Platforms.Select(t => new Platform
                {
                    Id = t.Id,
                    Name = t.Name,
                }).ToList(),
                Genres = g.Genres.Select(t => new Genre
                {
                    Id = t.Id,
                    Name = t.Name,
                }).ToList(),
                Tags = g.Tags.Select(t => new Tag
                {
                    Id = t.Id,
                    Name = t.Name,
                }).ToList(),
            }).ToList();
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

        private bool Save()
        {
            var saved = dataContext.SaveChanges();
            //var saved = 1;
            return saved > 0 ? true : false;
        }

    }
}
