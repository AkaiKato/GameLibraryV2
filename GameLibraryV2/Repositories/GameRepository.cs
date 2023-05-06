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

        private static void Filter(ref IQueryable<Game> games, FilterParameters filterParameters)
        {
            games = games.Where(g => g.NSFW == filterParameters.NSFW
                && g.ReleaseDate.Year >= filterParameters.MinYearOfRelease
                && g.ReleaseDate.Year <= filterParameters.MaxYearOfRelease
                && g.AveragePlayTime >= filterParameters.MinPlayTime
                && g.AveragePlayTime <= filterParameters.MaxPlayTime
                && g.Rating.TotalRating >= filterParameters.MinRating
                && g.Rating.TotalRating <= filterParameters.MaxRating)
                .AsQueryable();

            if (filterParameters.Status != null)
            {
                games = games.Where(g => filterParameters.Status
                                .Where(y => y != null)
                                .Select(y => y.ToLower())
                                .Contains(g.Status.ToLower()))
                                .AsQueryable();
            }

            if (filterParameters.Type != null)
            {
                games = games.Where(g => filterParameters.Type
                                .Where(y => y != null)
                                .Select(y => y.ToLower())
                                .Contains(g.Type.ToLower()))
                                .AsQueryable();
            }

            if (filterParameters.Genre != null)
            {
                foreach (var item in filterParameters.Genre)
                {
                    if (item == null)
                        continue;

                    var t = item.ToLower().Trim().ToString();
                    games = games.Where(g => g.Genres.Any(x => x.Name.ToLower().Contains(t))).AsQueryable();
                }
            }

            if (filterParameters.Tag != null)
            {
                foreach (var item in filterParameters.Tag)
                {
                    if (item == null)
                        continue;

                    var t = item.ToLower().Trim().ToString();
                    games = games.Where(g => g.Tags.Any(x => x.Name.ToLower().Contains(t))).AsQueryable();
                }
            }

            if (filterParameters.Platform != null)
            {
                foreach (var item in filterParameters.Platform)
                {
                    if (item == null)
                        continue;

                    var t = item.ToLower().Trim().ToString();
                    games = games.Where(g => g.Platforms.Any(x => x.Name.ToLower().Contains(t))).AsQueryable();
                }
            }

            if (filterParameters.Developer != null)
            {
                foreach (var item in filterParameters.Developer)
                {
                    if (item == null)
                        continue;

                    var t = item.ToLower().Trim().ToString();
                    games = games.Where(g => g.Developers.Any(x => x.Name.ToLower().Contains(t))).AsQueryable();
                }
            }

            if (filterParameters.Publisher != null)
            {
                foreach (var item in filterParameters.Publisher)
                {
                    if (item == null)
                        continue;

                    var t = item.ToLower().Trim().ToString();
                    games = games.Where(g => g.Publishers.Any(x => x.Name.ToLower().Contains(t))).AsQueryable();
                }
            }

            if (filterParameters.AgeRating != null)
            {
                games = games.Where(g => filterParameters.AgeRating
                                .Where(a => a != null)
                                .Select(a => a.ToLower())
                                .Contains(g.AgeRating.Name.Trim().ToLower()))
                                .AsQueryable();
            }

            games = games.Select(p => new Game
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
            });

            if (filterParameters.Rating)
                games = games.OrderByDescending(x => x.Rating.TotalRating);
            else
                games = games.OrderBy(x => x.Name);
        }

        public PagedList<Game> GetGames(FilterParameters filterParameters)
        {
            var games = dataContext.Games.AsQueryable();

            Filter(ref games, filterParameters);

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
                .Include(d => d.DLCs)!.ThenInclude(dg => dg.DLCGame)
                .Include(a => a.AgeRating).Include(d => d.Developers)
                .Include(p => p.Publishers).Include(p => p.Platforms)
                .Include(g => g.Genres).Include(t => t.Tags)
                .Include(p => p.ParentGame).Include(s => s.SystemRequirementsMin)
                .Include(s => s.SystemRequirementsMax).Include(r => r.Rating)
                .Where(x => x.Id == gameId).FirstOrDefault()!;
        }

        public Game GetGameByName(string gameName)
        {
            return dataContext.Games
                .Include(d => d.DLCs)!.ThenInclude(dg => dg.DLCGame)
                .Include(a => a.AgeRating).Include(d => d.Developers)
                .Include(p => p.Publishers).Include(p => p.Platforms)
                .Include(g => g.Genres).Include(t => t.Tags)
                .Include(p => p.ParentGame).Include(s => s.SystemRequirementsMin)
                .Include(s => s.SystemRequirementsMax).Include(r => r.Rating)
                .Where(x => x.Name.Trim().ToLower() == gameName.Trim().ToLower())
                .FirstOrDefault()!;
        }

        public Game GetDLCById(int gameId)
        {
            return dataContext.Games
                .Include(a => a.AgeRating).Include(d => d.Developers)
                .Include(p => p.Publishers).Include(p => p.Platforms)
                .Include(g => g.Genres).Include(t => t.Tags)
                .Include(p => p.ParentGame).Include(s => s.SystemRequirementsMin)
                .Include(s => s.SystemRequirementsMax).Include(r => r.Rating)
                .Where(x => x.Id == gameId && x.Type.Trim().ToLower() == "dlc".Trim().ToLower())
                .FirstOrDefault()!;
        }

        public Game GetDLCByName(string dlcName)
        {
            return dataContext.Games
                .Include(a => a.AgeRating).Include(d => d.Developers)
                .Include(p => p.Publishers).Include(p => p.Platforms)
                .Include(g => g.Genres).Include(t => t.Tags)
                .Include(p => p.ParentGame).Include(s => s.SystemRequirementsMin)
                .Include(s => s.SystemRequirementsMax).Include(r => r.Rating)
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

        public PagedList<Game> GetGamesByAgeRating(int ageRatingId, FilterParameters filterParameters)
        {
            var games = dataContext.Games.Where(g => g.AgeRating.Id == ageRatingId)
                .AsQueryable();

            Filter(ref games, filterParameters);

            return PagedList<Game>.ToPagedList(games.AsQueryable(), filterParameters.PageNumber, filterParameters.PageSize);
        }

        public PagedList<Game> GetGamesByDeveloper(int developerId, FilterParameters filterParameters)
        {
            var games = dataContext.Games.Where(g => g.Developers.Any(d => d.Id == developerId))
                .AsQueryable();

            Filter(ref games, filterParameters);

            return PagedList<Game>.ToPagedList(games.AsQueryable(), filterParameters.PageNumber, filterParameters.PageSize);
        }

        public PagedList<Game> GetGamesByGenre(int genreId, FilterParameters filterParameters)
        {
            var games = dataContext.Games.Where(g => g.Genres.Any(g => g.Id == genreId))
                .AsQueryable();

            Filter(ref games, filterParameters);

            return PagedList<Game>.ToPagedList(games.AsQueryable(), filterParameters.PageNumber, filterParameters.PageSize);
        }

        public PagedList<Game> GetGameByPlatform(int platformId, FilterParameters filterParameters)
        {
            var games = dataContext.Games
                .Where(g => g.Platforms.Any(g => g.Id == platformId))
                .AsQueryable();

            Filter(ref games, filterParameters);

            return PagedList<Game>.ToPagedList(games.AsQueryable(), filterParameters.PageNumber, filterParameters.PageSize);
        }

        public PagedList<Game> GetGamesByPublisher(int publisherId, FilterParameters filterParameters)
        {
            var games =  dataContext.Games
                .Where(g => g.Publishers.Any(p => p.Id == publisherId))
                .AsQueryable();

            Filter(ref games, filterParameters);

            return PagedList<Game>.ToPagedList(games.AsQueryable(), filterParameters.PageNumber, filterParameters.PageSize);
        }

        public PagedList<Game> GetGamesByTag(int tagId, FilterParameters filterParameters)
        {
            var games = dataContext.Games
                .Where(g => g.Tags.Any(g => g.Id == tagId))
                .AsQueryable();

            Filter(ref games, filterParameters);

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
