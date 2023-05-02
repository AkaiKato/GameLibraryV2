using GameLibraryV2.Data;
using GameLibraryV2.Helper;
using GameLibraryV2.Interfaces;
using GameLibraryV2.Models;
using GameLibraryV2.Models.Common;
using Microsoft.EntityFrameworkCore;

namespace GameLibraryV2.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly DataContext dataContext;
     
        public GameRepository(DataContext context)
        {
            dataContext = context;
        }

        public PagedList<Game> GetGamesOrderByRating(FilterParameters filterParameters)
        {
            var games = dataContext.Games
                .Where(g => g.NSFW == filterParameters.NSFW
                && g.ReleaseDate.Year >= filterParameters.MinYearOfRelease
                && g.ReleaseDate.Year <= filterParameters.MaxYearOfRelease
                && g.AveragePlayTime >= filterParameters.MinPlayTime
                && g.AveragePlayTime <= filterParameters.MaxPlayTime
                && g.Rating.TotalRating >= filterParameters.MinRating
                && g.Rating.TotalRating <= filterParameters.MaxRating)
                .Select(p => new Game
                {
                    Id = p.Id,
                    Name = p.Name,
                    PicturePath = p.PicturePath,
                    Status = p.Status,
                    ReleaseDate = p.ReleaseDate,
                    Description = p.Description,
                    AgeRating = p.AgeRating,
                    NSFW = p.NSFW,
                    Type = p.Type,
                    AveragePlayTime = p.AveragePlayTime,
                    Rating = p.Rating,
                    Developers = p.Developers.Select(t => new Developer
                    {
                        Id = t.Id,
                        Name = t.Name,
                    }).ToList(),
                    Publishers = p.Publishers.Select(t => new Publisher
                    {
                        Id = t.Id,
                        Name = t.Name,
                    }).ToList(),
                    Platforms = p.Platforms.Select(t => new Platform
                    {
                        Id = t.Id,
                        Name = t.Name,
                    }).ToList(),
                    Genres = p.Genres.Select(t => new Genre
                    {
                        Id = t.Id,
                        Name = t.Name,
                    }).ToList(),
                    Tags = p.Tags.Select(t => new Tag
                    {
                        Id = t.Id,
                        Name = t.Name,
                    }).ToList(),
                })
                .AsSplitQuery()
                .OrderByDescending(x => x.Rating.TotalRating)
                .AsEnumerable();

            if (filterParameters.Status != null)
                games = games.Where(g => filterParameters.Status
                                .Where(y => y != null)
                                .Select(y => y.ToLower())
                                .Contains(g.Status.ToLower()))
                                .AsEnumerable();

            if(filterParameters.Type != null)
                games = games.Where(g => filterParameters.Type
                                .Where(y => y != null)
                                .Select(y => y.ToLower())
                                .Contains(g.Type.ToLower()))
                                .AsEnumerable();

            if(filterParameters.Genre != null)
                games = games.Where(g => filterParameters.GenreEquals(g.Genres, filterParameters.Genre))
                                .AsEnumerable();

            if(filterParameters.Tag != null)
                games = games.Where(g => filterParameters.TagEquals(g.Tags, filterParameters.Tag))
                                .AsEnumerable();

            if (filterParameters.Platform != null)
                games = games.Where(g => filterParameters.PlatformEquals(g.Platforms, filterParameters.Platform))
                                .AsEnumerable();

            if (filterParameters.Developer != null)
                games = games.Where(g => filterParameters.DeveloperEquals(g.Developers, filterParameters.Developer))
                                .AsEnumerable();

            if(filterParameters.Publisher != null)
                games = games.Where(g => filterParameters.PublisherEquals(g.Publishers, filterParameters.Publisher))
                                .AsEnumerable();
            
            if(filterParameters.AgeRating != null)
                games = games.Where(g => filterParameters.AgeRating
                                .Where(a => a != null)
                                .Select(a => a.ToLower())
                                .Contains(g.AgeRating.Name.Trim().ToLower()))
                                .AsEnumerable();

            return PagedList<Game>.ToPagedList(games.AsQueryable(), filterParameters.PageNumber, filterParameters.PageSize);
           
        }

        public PagedList<Game> GetGamesOrderByName(FilterParameters filterParameters)
        {
            var games = dataContext.Games
                .Where(g => g.NSFW == filterParameters.NSFW
                && g.ReleaseDate.Year >= filterParameters.MinYearOfRelease
                && g.ReleaseDate.Year <= filterParameters.MaxYearOfRelease
                && g.AveragePlayTime >= filterParameters.MinPlayTime
                && g.AveragePlayTime <= filterParameters.MaxPlayTime
                && g.Rating.TotalRating >= filterParameters.MinRating
                && g.Rating.TotalRating <= filterParameters.MaxRating)
                .Select(p => new Game
                {
                    Id = p.Id,
                    Name = p.Name,
                    PicturePath = p.PicturePath,
                    Status = p.Status,
                    ReleaseDate = p.ReleaseDate,
                    Description = p.Description,
                    AgeRating = p.AgeRating,
                    NSFW = p.NSFW,
                    Type = p.Type,
                    AveragePlayTime = p.AveragePlayTime,
                    Rating = p.Rating,
                    Developers = p.Developers.Select(t => new Developer
                    {
                        Id = t.Id,
                        Name = t.Name,
                    }).ToList(),
                    Publishers = p.Publishers.Select(t => new Publisher
                    {
                        Id = t.Id,
                        Name = t.Name,
                    }).ToList(),
                    Platforms = p.Platforms.Select(t => new Platform
                    {
                        Id = t.Id,
                        Name = t.Name,
                    }).ToList(),
                    Genres = p.Genres.Select(t => new Genre
                    {
                        Id = t.Id,
                        Name = t.Name,
                    }).ToList(),
                    Tags = p.Tags.Select(t => new Tag
                    {
                        Id = t.Id,
                        Name = t.Name,
                    }).ToList(),
                })
                .OrderBy(x => x.Name)
                .AsEnumerable();

            if (filterParameters.Status != null)
                games = games.Where(g => filterParameters.Status
                                .Where(y => y != null)
                                .Select(y => y.ToLower())
                                .Contains(g.Status.ToLower()))
                                .AsEnumerable();

            if (filterParameters.Type != null)
                games = games.Where(g => filterParameters.Type
                                .Where(y => y != null)
                                .Select(y => y.ToLower())
                                .Contains(g.Type.ToLower()))
                                .AsEnumerable();

            if (filterParameters.Genre != null)
                games = games.Where(g => filterParameters.GenreEquals(g.Genres, filterParameters.Genre))
                                .AsEnumerable();

            if (filterParameters.Tag != null)
                games = games.Where(g => filterParameters.TagEquals(g.Tags, filterParameters.Tag))
                                .AsEnumerable();

            if (filterParameters.Platform != null)
                games = games.Where(g => filterParameters.PlatformEquals(g.Platforms, filterParameters.Platform))
                                .AsEnumerable();

            if (filterParameters.Developer != null)
                games = games.Where(g => filterParameters.DeveloperEquals(g.Developers, filterParameters.Developer))
                                .AsEnumerable();

            if (filterParameters.Publisher != null)
                games = games.Where(g => filterParameters.PublisherEquals(g.Publishers, filterParameters.Publisher))
                                .AsEnumerable();

            if (filterParameters.AgeRating != null)
                games = games.Where(g => filterParameters.AgeRating
                                .Where(a => a != null)
                                .Select(a => a.ToLower())
                                .Contains(g.AgeRating.Name.Trim().ToLower()))
                                .AsEnumerable();

            return PagedList<Game>.ToPagedList(games.AsQueryable(), filterParameters.PageNumber, filterParameters.PageSize);

        }

        public bool GameExists(int gameId)
        {
            return dataContext.Games.Any(x => x.Id == gameId);
        }

        public bool GameNameAlreadyInUse(int gameId, string gameName)
        {
            return dataContext.Games.Any(g => g.Name.Trim().ToLower() == gameName.Trim().ToLower() && g.Id != gameId );
        }

        public bool DLCExists(int dlcId)
        {
            return dataContext.Games.Any(g => g.Id == dlcId && g.Type.ToLower() == "dlc");
        }

        public Game GetGameById(int gameId)
        {
            return dataContext.Games
                .Include(d => d.DLCs)!
                .ThenInclude(dg => dg.DLCGame)
                .Include(a => a.AgeRating)
                .Include(d => d.Developers)
                .Include(p => p.Publishers)
                .Include(p => p.Platforms)
                .Include(g => g.Genres)
                .Include(t => t.Tags)
                .Include(p => p.ParentGame)
                .Include(s => s.SystemRequirementsMin)
                .Include(s => s.SystemRequirementsMax)
                .Include(r => r.Rating)
                .Where(x => x.Id == gameId)
                .FirstOrDefault()!;
        }

        public Game GetGameByName(string gameName)
        {
            return dataContext.Games
                .Include(d => d.DLCs)!
                .ThenInclude(dg => dg.DLCGame)
                .Include(a => a.AgeRating)
                .Include(d => d.Developers)
                .Include(p => p.Publishers)
                .Include(p => p.Platforms)
                .Include(g => g.Genres)
                .Include(t => t.Tags)
                .Include(p => p.ParentGame)
                .Include(s => s.SystemRequirementsMin)
                .Include(s => s.SystemRequirementsMax)
                .Include(r => r.Rating)
                .Where(x => x.Name.Trim().ToLower() == gameName.Trim().ToLower())
                .FirstOrDefault()!;
        }

        public Game GetDLCById(int gameId)
        {
            return dataContext.Games
                .Include(a => a.AgeRating)
                .Include(d => d.Developers)
                .Include(p => p.Publishers)
                .Include(p => p.Platforms)
                .Include(g => g.Genres)
                .Include(t => t.Tags)
                .Include(p => p.ParentGame)
                .Include(s => s.SystemRequirementsMin)
                .Include(s => s.SystemRequirementsMax)
                .Include(r => r.Rating)
                .Where(x => x.Id == gameId && x.Type.Trim().ToLower() == "dlc".Trim().ToLower())
                .FirstOrDefault()!;
        }

        public Game GetDLCByName(string dlcName)
        {
            return dataContext.Games
                .Include(a => a.AgeRating)
                .Include(d => d.Developers)
                .Include(p => p.Publishers)
                .Include(p => p.Platforms)
                .Include(g => g.Genres)
                .Include(t => t.Tags)
                .Include(p => p.ParentGame)
                .Include(s => s.SystemRequirementsMin)
                .Include(s => s.SystemRequirementsMax)
                .Include(r => r.Rating)
                .Where(g => g.Name == dlcName && g.Type.ToLower() == "dlc")
                .FirstOrDefault()!;
        }

        public IList<Game> GetDLCs()
        {
            return dataContext.Games.Where(d => d.Type == "DLC").Select(g => new Game
            {
                Id = g.Id,
                Name = g.Name,
                ReleaseDate = g.ReleaseDate,
                Status = g.Status,
                Description = g.Description,
                AgeRating = g.AgeRating,
                NSFW = g.NSFW,
                Type = g.Type,
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

        public PagedList<Game> GetGamesByAgeRatingOrderByRating(int ageRatingId, FilterParameters filterParameters)
        {
            var games =  dataContext.Games
                .Include(a => a.AgeRating)
                .Where(g => g.AgeRating.Id == ageRatingId
                && g.NSFW == filterParameters.NSFW
                && g.ReleaseDate.Year >= filterParameters.MinYearOfRelease
                && g.ReleaseDate.Year <= filterParameters.MaxYearOfRelease
                && g.AveragePlayTime >= filterParameters.MinPlayTime
                && g.AveragePlayTime <= filterParameters.MaxPlayTime
                && g.Rating.TotalRating >= filterParameters.MinRating
                && g.Rating.TotalRating <= filterParameters.MaxRating)
                .Select(g => new Game {
                    Id = g.Id,
                    Name = g.Name,
                    PicturePath = g.PicturePath,
                    Status = g.Status,
                    ReleaseDate = g.ReleaseDate,
                    Description = g.Description,
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
                }).OrderByDescending(x => x.Rating.TotalRating)
                .AsEnumerable();

            if (filterParameters.Status != null)
                games = games.Where(g => filterParameters.Status
                                .Where(y => y != null)
                                .Select(y => y.ToLower())
                                .Contains(g.Status.ToLower()))
                                .AsEnumerable();

            if (filterParameters.Type != null)
                games = games.Where(g => filterParameters.Type
                                .Where(y => y != null)
                                .Select(y => y.ToLower())
                                .Contains(g.Type.ToLower()))
                                .AsEnumerable();

            if (filterParameters.Genre != null)
                games = games.Where(g => filterParameters.GenreEquals(g.Genres, filterParameters.Genre))
                                .AsEnumerable();

            if (filterParameters.Tag != null)
                games = games.Where(g => filterParameters.TagEquals(g.Tags, filterParameters.Tag))
                                .AsEnumerable();

            if (filterParameters.Platform != null)
                games = games.Where(g => filterParameters.PlatformEquals(g.Platforms, filterParameters.Platform))
                                .AsEnumerable();

            if (filterParameters.Developer != null)
                games = games.Where(g => filterParameters.DeveloperEquals(g.Developers, filterParameters.Developer))
                                .AsEnumerable();

            if (filterParameters.Publisher != null)
                games = games.Where(g => filterParameters.PublisherEquals(g.Publishers, filterParameters.Publisher))
                                .AsEnumerable();

            return PagedList<Game>.ToPagedList(games.AsQueryable(), filterParameters.PageNumber, filterParameters.PageSize);
        }

        public PagedList<Game> GetGamesByAgeRatingOrderByName(int ageRatingId, FilterParameters filterParameters)
        {
            var games = dataContext.Games
                .Include(a => a.AgeRating)
                .Where(g => g.AgeRating.Id == ageRatingId
                && g.NSFW == filterParameters.NSFW
                && g.ReleaseDate.Year >= filterParameters.MinYearOfRelease
                && g.ReleaseDate.Year <= filterParameters.MaxYearOfRelease
                && g.AveragePlayTime >= filterParameters.MinPlayTime
                && g.AveragePlayTime <= filterParameters.MaxPlayTime
                && g.Rating.TotalRating >= filterParameters.MinRating
                && g.Rating.TotalRating <= filterParameters.MaxRating)
                .Select(g => new Game
                {
                    Id = g.Id,
                    Name = g.Name,
                    PicturePath = g.PicturePath,
                    Status = g.Status,
                    ReleaseDate = g.ReleaseDate,
                    Description = g.Description,
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
                }).OrderBy(x => x.Name)
                .AsEnumerable();

            if (filterParameters.Status != null)
                games = games.Where(g => filterParameters.Status
                                .Where(y => y != null)
                                .Select(y => y.ToLower())
                                .Contains(g.Status.ToLower()))
                                .AsEnumerable();

            if (filterParameters.Type != null)
                games = games.Where(g => filterParameters.Type
                                .Where(y => y != null)
                                .Select(y => y.ToLower())
                                .Contains(g.Type.ToLower()))
                                .AsEnumerable();

            if (filterParameters.Genre != null)
                games = games.Where(g => filterParameters.GenreEquals(g.Genres, filterParameters.Genre))
                                .AsEnumerable();

            if (filterParameters.Tag != null)
                games = games.Where(g => filterParameters.TagEquals(g.Tags, filterParameters.Tag))
                                .AsEnumerable();

            if (filterParameters.Platform != null)
                games = games.Where(g => filterParameters.PlatformEquals(g.Platforms, filterParameters.Platform))
                                .AsEnumerable();

            if (filterParameters.Developer != null)
                games = games.Where(g => filterParameters.DeveloperEquals(g.Developers, filterParameters.Developer))
                                .AsEnumerable();

            if (filterParameters.Publisher != null)
                games = games.Where(g => filterParameters.PublisherEquals(g.Publishers, filterParameters.Publisher))
                                .AsEnumerable();

            return PagedList<Game>.ToPagedList(games.AsQueryable(), filterParameters.PageNumber, filterParameters.PageSize);
        }

        public PagedList<Game> GetGamesByDeveloperOrderByRating(int developerId, FilterParameters filterParameters)
        {
            var games = dataContext.Games.
                Where(g => g.Developers.Any(d => d.Id == developerId)
                && g.NSFW == filterParameters.NSFW
                && g.ReleaseDate.Year >= filterParameters.MinYearOfRelease
                && g.ReleaseDate.Year <= filterParameters.MaxYearOfRelease
                && g.AveragePlayTime >= filterParameters.MinPlayTime
                && g.AveragePlayTime <= filterParameters.MaxPlayTime
                && g.Rating.TotalRating >= filterParameters.MinRating
                && g.Rating.TotalRating <= filterParameters.MaxRating)
                .Select(g => new Game {
                    Id = g.Id,
                    Name = g.Name,
                    PicturePath = g.PicturePath,
                    Status = g.Status,
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
                })
                .OrderByDescending(x => x.Rating.TotalRating)
                .AsEnumerable();

            if (filterParameters.Status != null)
                games = games.Where(g => filterParameters.Status
                                .Where(y => y != null)
                                .Select(y => y.ToLower())
                                .Contains(g.Status.ToLower()))
                                .AsEnumerable();

            if (filterParameters.Type != null)
                games = games.Where(g => filterParameters.Type
                                .Where(y => y != null)
                                .Select(y => y.ToLower())
                                .Contains(g.Type.ToLower()))
                                .AsEnumerable();

            if (filterParameters.Genre != null)
                games = games.Where(g => filterParameters.GenreEquals(g.Genres, filterParameters.Genre))
                                .AsEnumerable();

            if (filterParameters.Tag != null)
                games = games.Where(g => filterParameters.TagEquals(g.Tags, filterParameters.Tag))
                                .AsEnumerable();

            if (filterParameters.Platform != null)
                games = games.Where(g => filterParameters.PlatformEquals(g.Platforms, filterParameters.Platform))
                                .AsEnumerable();

            if (filterParameters.Publisher != null)
                games = games.Where(g => filterParameters.PublisherEquals(g.Publishers, filterParameters.Publisher))
                                .AsEnumerable();

            if (filterParameters.AgeRating != null)
                games = games.Where(g => filterParameters.AgeRating
                                .Where(a => a != null)
                                .Select(a => a.ToLower())
                                .Contains(g.AgeRating.Name.Trim().ToLower()))
                                .AsEnumerable();

            return PagedList<Game>.ToPagedList(games.AsQueryable(), filterParameters.PageNumber, filterParameters.PageSize);
        }

        public PagedList<Game> GetGamesByDeveloperOrderByName(int developerId, FilterParameters filterParameters)
        {
            var games = dataContext.Games.
                Where(g => g.Developers.Any(d => d.Id == developerId)
                && g.NSFW == filterParameters.NSFW
                && g.ReleaseDate.Year >= filterParameters.MinYearOfRelease
                && g.ReleaseDate.Year <= filterParameters.MaxYearOfRelease
                && g.AveragePlayTime >= filterParameters.MinPlayTime
                && g.AveragePlayTime <= filterParameters.MaxPlayTime
                && g.Rating.TotalRating >= filterParameters.MinRating
                && g.Rating.TotalRating <= filterParameters.MaxRating)
                .Select(g => new Game
                {
                    Id = g.Id,
                    Name = g.Name,
                    PicturePath = g.PicturePath,
                    Status = g.Status,
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
                })
                .OrderBy(x => x.Name)
                .AsEnumerable();

            if (filterParameters.Status != null)
                games = games.Where(g => filterParameters.Status
                                .Where(y => y != null)
                                .Select(y => y.ToLower())
                                .Contains(g.Status.ToLower()))
                                .AsEnumerable();

            if (filterParameters.Type != null)
                games = games.Where(g => filterParameters.Type
                                .Where(y => y != null)
                                .Select(y => y.ToLower())
                                .Contains(g.Type.ToLower()))
                                .AsEnumerable();

            if (filterParameters.Genre != null)
                games = games.Where(g => filterParameters.GenreEquals(g.Genres, filterParameters.Genre))
                                .AsEnumerable();

            if (filterParameters.Tag != null)
                games = games.Where(g => filterParameters.TagEquals(g.Tags, filterParameters.Tag))
                                .AsEnumerable();

            if (filterParameters.Platform != null)
                games = games.Where(g => filterParameters.PlatformEquals(g.Platforms, filterParameters.Platform))
                                .AsEnumerable();

            if (filterParameters.Publisher != null)
                games = games.Where(g => filterParameters.PublisherEquals(g.Publishers, filterParameters.Publisher))
                                .AsEnumerable();

            if (filterParameters.AgeRating != null)
                games = games.Where(g => filterParameters.AgeRating
                                .Where(a => a != null)
                                .Select(a => a.ToLower())
                                .Contains(g.AgeRating.Name.Trim().ToLower()))
                                .AsEnumerable();

            return PagedList<Game>.ToPagedList(games.AsQueryable(), filterParameters.PageNumber, filterParameters.PageSize);
        }

        public PagedList<Game> GetGamesByGenreOrderByRating(int genreId, FilterParameters filterParameters)
        {
            var games = dataContext.Games
                .Where(g => g.Genres.Any(g => g.Id == genreId)
                && g.NSFW == filterParameters.NSFW
                && g.ReleaseDate.Year >= filterParameters.MinYearOfRelease
                && g.ReleaseDate.Year <= filterParameters.MaxYearOfRelease
                && g.AveragePlayTime >= filterParameters.MinPlayTime
                && g.AveragePlayTime <= filterParameters.MaxPlayTime
                && g.Rating.TotalRating >= filterParameters.MinRating
                && g.Rating.TotalRating <= filterParameters.MaxRating)
                .Select(g => new Game {
                    Id = g.Id,
                    Name = g.Name,
                    PicturePath = g.PicturePath,
                    Status = g.Status,
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
                })
                .OrderByDescending(x => x.Rating.TotalRating)
                .AsEnumerable();

            if (filterParameters.Status != null)
                games = games.Where(g => filterParameters.Status
                                .Where(y => y != null)
                                .Select(y => y.ToLower())
                                .Contains(g.Status.ToLower()))
                                .AsEnumerable();

            if (filterParameters.Type != null)
                games = games.Where(g => filterParameters.Type
                                .Where(y => y != null)
                                .Select(y => y.ToLower())
                                .Contains(g.Type.ToLower()))
                                .AsEnumerable();

            if (filterParameters.Tag != null)
                games = games.Where(g => filterParameters.TagEquals(g.Tags, filterParameters.Tag))
                                .AsEnumerable();

            if (filterParameters.Platform != null)
                games = games.Where(g => filterParameters.PlatformEquals(g.Platforms, filterParameters.Platform))
                                .AsEnumerable();

            if (filterParameters.Developer != null)
                games = games.Where(g => filterParameters.DeveloperEquals(g.Developers, filterParameters.Developer))
                                .AsEnumerable();

            if (filterParameters.Publisher != null)
                games = games.Where(g => filterParameters.PublisherEquals(g.Publishers, filterParameters.Publisher))
                                .AsEnumerable();

            if (filterParameters.AgeRating != null)
                games = games.Where(g => filterParameters.AgeRating
                                .Where(a => a != null)
                                .Select(a => a.ToLower())
                                .Contains(g.AgeRating.Name.Trim().ToLower()))
                                .AsEnumerable();

            return PagedList<Game>.ToPagedList(games.AsQueryable(), filterParameters.PageNumber, filterParameters.PageSize);
        }

        public PagedList<Game> GetGamesByGenreOrderByName(int genreId, FilterParameters filterParameters)
        {
            var games = dataContext.Games
                .Where(g => g.Genres.Any(g => g.Id == genreId)
                && g.NSFW == filterParameters.NSFW
                && g.ReleaseDate.Year >= filterParameters.MinYearOfRelease
                && g.ReleaseDate.Year <= filterParameters.MaxYearOfRelease
                && g.AveragePlayTime >= filterParameters.MinPlayTime
                && g.AveragePlayTime <= filterParameters.MaxPlayTime
                && g.Rating.TotalRating >= filterParameters.MinRating
                && g.Rating.TotalRating <= filterParameters.MaxRating)
                .Select(g => new Game
                {
                    Id = g.Id,
                    Name = g.Name,
                    PicturePath = g.PicturePath,
                    Status = g.Status,
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
                })
                .OrderBy(x => x.Name)
                .AsEnumerable();

            if (filterParameters.Status != null)
                games = games.Where(g => filterParameters.Status
                                .Where(y => y != null)
                                .Select(y => y.ToLower())
                                .Contains(g.Status.ToLower()))
                                .AsEnumerable();

            if (filterParameters.Type != null)
                games = games.Where(g => filterParameters.Type
                                .Where(y => y != null)
                                .Select(y => y.ToLower())
                                .Contains(g.Type.ToLower()))
                                .AsEnumerable();

            if (filterParameters.Tag != null)
                games = games.Where(g => filterParameters.TagEquals(g.Tags, filterParameters.Tag))
                                .AsEnumerable();

            if (filterParameters.Platform != null)
                games = games.Where(g => filterParameters.PlatformEquals(g.Platforms, filterParameters.Platform))
                                .AsEnumerable();

            if (filterParameters.Developer != null)
                games = games.Where(g => filterParameters.DeveloperEquals(g.Developers, filterParameters.Developer))
                                .AsEnumerable();

            if (filterParameters.Publisher != null)
                games = games.Where(g => filterParameters.PublisherEquals(g.Publishers, filterParameters.Publisher))
                                .AsEnumerable();

            if (filterParameters.AgeRating != null)
                games = games.Where(g => filterParameters.AgeRating
                                .Where(a => a != null)
                                .Select(a => a.ToLower())
                                .Contains(g.AgeRating.Name.Trim().ToLower()))
                                .AsEnumerable();

            return PagedList<Game>.ToPagedList(games.AsQueryable(), filterParameters.PageNumber, filterParameters.PageSize);
        }

        public PagedList<Game> GetGameByPlatformOrderByRating(int platformId, FilterParameters filterParameters)
        {
            var games = dataContext.Games
                .Where(g => g.Platforms.Any(g => g.Id == platformId)
                && g.NSFW == filterParameters.NSFW
                && g.ReleaseDate.Year >= filterParameters.MinYearOfRelease
                && g.ReleaseDate.Year <= filterParameters.MaxYearOfRelease
                && g.AveragePlayTime >= filterParameters.MinPlayTime
                && g.AveragePlayTime <= filterParameters.MaxPlayTime
                && g.Rating.TotalRating >= filterParameters.MinRating
                && g.Rating.TotalRating <= filterParameters.MaxRating)
                .Select(g => new Game {
                    Id = g.Id,
                    Name = g.Name,
                    PicturePath = g.PicturePath,
                    Status = g.Status,
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
                }).OrderByDescending(x => x.Rating.TotalRating)
                .AsEnumerable();

            if (filterParameters.Status != null)
                games = games.Where(g => filterParameters.Status
                                .Where(y => y != null)
                                .Select(y => y.ToLower())
                                .Contains(g.Status.ToLower()))
                                .AsEnumerable();

            if (filterParameters.Type != null)
                games = games.Where(g => filterParameters.Type
                                .Where(y => y != null)
                                .Select(y => y.ToLower())
                                .Contains(g.Type.ToLower()))
                                .AsEnumerable();

            if (filterParameters.Genre != null)
                games = games.Where(g => filterParameters.GenreEquals(g.Genres, filterParameters.Genre))
                                .AsEnumerable();

            if (filterParameters.Tag != null)
                games = games.Where(g => filterParameters.TagEquals(g.Tags, filterParameters.Tag))
                                .AsEnumerable();

            if (filterParameters.Developer != null)
                games = games.Where(g => filterParameters.DeveloperEquals(g.Developers, filterParameters.Developer))
                                .AsEnumerable();

            if (filterParameters.Publisher != null)
                games = games.Where(g => filterParameters.PublisherEquals(g.Publishers, filterParameters.Publisher))
                                .AsEnumerable();

            if (filterParameters.AgeRating != null)
                games = games.Where(g => filterParameters.AgeRating
                                .Where(a => a != null)
                                .Select(a => a.ToLower())
                                .Contains(g.AgeRating.Name.Trim().ToLower()))
                                .AsEnumerable();

            return PagedList<Game>.ToPagedList(games.AsQueryable(), filterParameters.PageNumber, filterParameters.PageSize);

        }

        public PagedList<Game> GetGameByPlatformOrderByName(int platformId, FilterParameters filterParameters)
        {
            var games = dataContext.Games
                .Where(g => g.Platforms.Any(g => g.Id == platformId)
                && g.NSFW == filterParameters.NSFW
                && g.ReleaseDate.Year >= filterParameters.MinYearOfRelease
                && g.ReleaseDate.Year <= filterParameters.MaxYearOfRelease
                && g.AveragePlayTime >= filterParameters.MinPlayTime
                && g.AveragePlayTime <= filterParameters.MaxPlayTime
                && g.Rating.TotalRating >= filterParameters.MinRating
                && g.Rating.TotalRating <= filterParameters.MaxRating)
                .Select(g => new Game
                {
                    Id = g.Id,
                    Name = g.Name,
                    PicturePath = g.PicturePath,
                    Status = g.Status,
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
                }).OrderBy(x => x.Name)
                .AsEnumerable();

            if (filterParameters.Status != null)
                games = games.Where(g => filterParameters.Status
                                .Where(y => y != null)
                                .Select(y => y.ToLower())
                                .Contains(g.Status.ToLower()))
                                .AsEnumerable();

            if (filterParameters.Type != null)
                games = games.Where(g => filterParameters.Type
                                .Where(y => y != null)
                                .Select(y => y.ToLower())
                                .Contains(g.Type.ToLower()))
                                .AsEnumerable();

            if (filterParameters.Genre != null)
                games = games.Where(g => filterParameters.GenreEquals(g.Genres, filterParameters.Genre))
                                .AsEnumerable();

            if (filterParameters.Tag != null)
                games = games.Where(g => filterParameters.TagEquals(g.Tags, filterParameters.Tag))
                                .AsEnumerable();

            if (filterParameters.Developer != null)
                games = games.Where(g => filterParameters.DeveloperEquals(g.Developers, filterParameters.Developer))
                                .AsEnumerable();

            if (filterParameters.Publisher != null)
                games = games.Where(g => filterParameters.PublisherEquals(g.Publishers, filterParameters.Publisher))
                                .AsEnumerable();

            if (filterParameters.AgeRating != null)
                games = games.Where(g => filterParameters.AgeRating
                                .Where(a => a != null)
                                .Select(a => a.ToLower())
                                .Contains(g.AgeRating.Name.Trim().ToLower()))
                                .AsEnumerable();

            return PagedList<Game>.ToPagedList(games.AsQueryable(), filterParameters.PageNumber, filterParameters.PageSize);
        }

        public PagedList<Game> GetGamesByPublisherOrderByRating(int publisherId, FilterParameters filterParameters)
        {
            var games =  dataContext.Games
                .Where(g => g.Publishers.Any(p => p.Id == publisherId)
                 && g.NSFW == filterParameters.NSFW
                && g.ReleaseDate.Year >= filterParameters.MinYearOfRelease
                && g.ReleaseDate.Year <= filterParameters.MaxYearOfRelease
                && g.AveragePlayTime >= filterParameters.MinPlayTime
                && g.AveragePlayTime <= filterParameters.MaxPlayTime
                && g.Rating.TotalRating >= filterParameters.MinRating
                && g.Rating.TotalRating <= filterParameters.MaxRating)
                .Select(g => new Game {
                    Id = g.Id,
                    Name = g.Name,
                    PicturePath = g.PicturePath,
                    Status = g.Status,
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
                }).OrderByDescending(x => x.Rating.TotalRating)
                .AsEnumerable();

            if (filterParameters.Status != null)
                games = games.Where(g => filterParameters.Status
                                .Where(y => y != null)
                                .Select(y => y.ToLower())
                                .Contains(g.Status.ToLower()))
                                .AsEnumerable();

            if (filterParameters.Type != null)
                games = games.Where(g => filterParameters.Type
                                .Where(y => y != null)
                                .Select(y => y.ToLower())
                                .Contains(g.Type.ToLower()))
                                .AsEnumerable();

            if (filterParameters.Genre != null)
                games = games.Where(g => filterParameters.GenreEquals(g.Genres, filterParameters.Genre))
                                .AsEnumerable();

            if (filterParameters.Tag != null)
                games = games.Where(g => filterParameters.TagEquals(g.Tags, filterParameters.Tag))
                                .AsEnumerable();

            if (filterParameters.Platform != null)
                games = games.Where(g => filterParameters.PlatformEquals(g.Platforms, filterParameters.Platform))
                                .AsEnumerable();

            if (filterParameters.Developer != null)
                games = games.Where(g => filterParameters.DeveloperEquals(g.Developers, filterParameters.Developer))
                                .AsEnumerable();

            if (filterParameters.AgeRating != null)
                games = games.Where(g => filterParameters.AgeRating
                                .Where(a => a != null)
                                .Select(a => a.ToLower())
                                .Contains(g.AgeRating.Name.Trim().ToLower()))
                                .AsEnumerable();

            return PagedList<Game>.ToPagedList(games.AsQueryable(), filterParameters.PageNumber, filterParameters.PageSize);
        }

        public PagedList<Game> GetGamesByPublisherOrderByName(int publisherId, FilterParameters filterParameters)
        {
            var games = dataContext.Games
                .Where(g => g.Publishers.Any(p => p.Id == publisherId)
                && g.NSFW == filterParameters.NSFW
                && g.ReleaseDate.Year >= filterParameters.MinYearOfRelease
                && g.ReleaseDate.Year <= filterParameters.MaxYearOfRelease
                && g.AveragePlayTime >= filterParameters.MinPlayTime
                && g.AveragePlayTime <= filterParameters.MaxPlayTime
                && g.Rating.TotalRating >= filterParameters.MinRating
                && g.Rating.TotalRating <= filterParameters.MaxRating)
                .Select(g => new Game
                {
                    Id = g.Id,
                    Name = g.Name,
                    PicturePath = g.PicturePath,
                    Status = g.Status,
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
                }).OrderBy(x => x.Name)
                .AsEnumerable();

            if (filterParameters.Status != null)
                games = games.Where(g => filterParameters.Status
                                .Where(y => y != null)
                                .Select(y => y.ToLower())
                                .Contains(g.Status.ToLower()))
                                .AsEnumerable();

            if (filterParameters.Type != null)
                games = games.Where(g => filterParameters.Type
                                .Where(y => y != null)
                                .Select(y => y.ToLower())
                                .Contains(g.Type.ToLower()))
                                .AsEnumerable();

            if (filterParameters.Genre != null)
                games = games.Where(g => filterParameters.GenreEquals(g.Genres, filterParameters.Genre))
                                .AsEnumerable();

            if (filterParameters.Tag != null)
                games = games.Where(g => filterParameters.TagEquals(g.Tags, filterParameters.Tag))
                                .AsEnumerable();

            if (filterParameters.Platform != null)
                games = games.Where(g => filterParameters.PlatformEquals(g.Platforms, filterParameters.Platform))
                                .AsEnumerable();

            if (filterParameters.Developer != null)
                games = games.Where(g => filterParameters.DeveloperEquals(g.Developers, filterParameters.Developer))
                                .AsEnumerable();

            if (filterParameters.AgeRating != null)
                games = games.Where(g => filterParameters.AgeRating
                                .Where(a => a != null)
                                .Select(a => a.ToLower())
                                .Contains(g.AgeRating.Name.Trim().ToLower()))
                                .AsEnumerable();

            return PagedList<Game>.ToPagedList(games.AsQueryable(), filterParameters.PageNumber, filterParameters.PageSize);
        }

        public PagedList<Game> GetGamesByTagOrderByRating(int tagId, FilterParameters filterParameters)
        {
            var games = dataContext.Games
                .Where(g => g.Tags.Any(g => g.Id == tagId)
                && g.NSFW == filterParameters.NSFW
                && g.ReleaseDate.Year >= filterParameters.MinYearOfRelease
                && g.ReleaseDate.Year <= filterParameters.MaxYearOfRelease
                && g.AveragePlayTime >= filterParameters.MinPlayTime
                && g.AveragePlayTime <= filterParameters.MaxPlayTime
                && g.Rating.TotalRating >= filterParameters.MinRating
                && g.Rating.TotalRating <= filterParameters.MaxRating)
                .Select(g => new Game {
                    Id = g.Id,
                    Name = g.Name,
                    PicturePath = g.PicturePath,
                    Status = g.Status,
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
                }).OrderByDescending(x => x.Rating.TotalRating)
                .AsEnumerable();

            if (filterParameters.Status != null)
                games = games.Where(g => filterParameters.Status
                                .Where(y => y != null)
                                .Select(y => y.ToLower())
                                .Contains(g.Status.ToLower()))
                                .AsEnumerable();

            if (filterParameters.Type != null)
                games = games.Where(g => filterParameters.Type
                                .Where(y => y != null)
                                .Select(y => y.ToLower())
                                .Contains(g.Type.ToLower()))
                                .AsEnumerable();

            if (filterParameters.Genre != null)
                games = games.Where(g => filterParameters.GenreEquals(g.Genres, filterParameters.Genre))
                                .AsEnumerable();

            if (filterParameters.Platform != null)
                games = games.Where(g => filterParameters.PlatformEquals(g.Platforms, filterParameters.Platform))
                                .AsEnumerable();

            if (filterParameters.Developer != null)
                games = games.Where(g => filterParameters.DeveloperEquals(g.Developers, filterParameters.Developer))
                                .AsEnumerable();

            if (filterParameters.Publisher != null)
                games = games.Where(g => filterParameters.PublisherEquals(g.Publishers, filterParameters.Publisher))
                                .AsEnumerable();

            if (filterParameters.AgeRating != null)
                games = games.Where(g => filterParameters.AgeRating
                                .Where(a => a != null)
                                .Select(a => a.ToLower())
                                .Contains(g.AgeRating.Name.Trim().ToLower()))
                                .AsEnumerable();

            return PagedList<Game>.ToPagedList(games.AsQueryable(), filterParameters.PageNumber, filterParameters.PageSize);
        }

        public PagedList<Game> GetGamesByTagOrderByName(int tagId, FilterParameters filterParameters)
        {
            var games = dataContext.Games
                .Where(g => g.Tags.Any(g => g.Id == tagId)
                && g.NSFW == filterParameters.NSFW
                && g.ReleaseDate.Year >= filterParameters.MinYearOfRelease
                && g.ReleaseDate.Year <= filterParameters.MaxYearOfRelease
                && g.AveragePlayTime >= filterParameters.MinPlayTime
                && g.AveragePlayTime <= filterParameters.MaxPlayTime
                && g.Rating.TotalRating >= filterParameters.MinRating
                && g.Rating.TotalRating <= filterParameters.MaxRating)
                .Select(g => new Game
                {
                    Id = g.Id,
                    Name = g.Name,
                    PicturePath = g.PicturePath,
                    Status = g.Status,
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
                }).OrderBy(x => x.Name)
                .AsEnumerable();

            if (filterParameters.Status != null)
                games = games.Where(g => filterParameters.Status
                                .Where(y => y != null)
                                .Select(y => y.ToLower())
                                .Contains(g.Status.ToLower()))
                                .AsEnumerable();

            if (filterParameters.Type != null)
                games = games.Where(g => filterParameters.Type
                                .Where(y => y != null)
                                .Select(y => y.ToLower())
                                .Contains(g.Type.ToLower()))
                                .AsEnumerable();

            if (filterParameters.Genre != null)
                games = games.Where(g => filterParameters.GenreEquals(g.Genres, filterParameters.Genre))
                                .AsEnumerable();

            if (filterParameters.Platform != null)
                games = games.Where(g => filterParameters.PlatformEquals(g.Platforms, filterParameters.Platform))
                                .AsEnumerable();

            if (filterParameters.Developer != null)
                games = games.Where(g => filterParameters.DeveloperEquals(g.Developers, filterParameters.Developer))
                                .AsEnumerable();

            if (filterParameters.Publisher != null)
                games = games.Where(g => filterParameters.PublisherEquals(g.Publishers, filterParameters.Publisher))
                                .AsEnumerable();

            if (filterParameters.AgeRating != null)
                games = games.Where(g => filterParameters.AgeRating
                                .Where(a => a != null)
                                .Select(a => a.ToLower())
                                .Contains(g.AgeRating.Name.Trim().ToLower()))
                                .AsEnumerable();

            return PagedList<Game>.ToPagedList(games.AsQueryable(), filterParameters.PageNumber, filterParameters.PageSize);
        }

        public bool CreateGame(Game game)
        {
            dataContext.Add(game);
            return Save();
        }

        public bool UpdateGame(Game game)
        {
            dataContext.Update(game);
            return Save();
        }

        public bool DeleteGame(Game game)
        {
            dataContext.Remove(game);
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
