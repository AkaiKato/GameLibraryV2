using GameLibraryV2.Data;
using GameLibraryV2.Interfaces;
using GameLibraryV2.Models;
using Microsoft.EntityFrameworkCore;

namespace GameLibraryV2.Repositories
{
    public class DLCRepository : IDLCRepository
    {
        private readonly DataContext dataContext;

        public DLCRepository(DataContext context)
        {
            dataContext = context;
        }

        public bool DLCExists(int dlcId)
        {
            return dataContext.Games.Any(g => g.Id == dlcId && g.Type.ToLower() == "dlc");
        }

        public Game GetDLCById(int dlcId)
        {
            return dataContext.Games.Include(d => d.Developers).Include(p => p.Publishers).
                Include(p => p.Platforms).Include(g => g.Genres).Include(t => t.Tags).Include(p => p.ParentGame).
                Include(s => s.SystemRequirementsMin).Include(s => s.SystemRequirementsMax).Include(r => r.Rating).
                Where(g => g.Id == dlcId && g.Type.ToLower() == "dlc").FirstOrDefault()!;
        }

        public Game GetDLCByName(string dlcName)
        {
            return dataContext.Games.Include(d => d.Developers).Include(p => p.Publishers).
                Include(p => p.Platforms).Include(g => g.Genres).Include(t => t.Tags).Include(p => p.ParentGame).
                Include(s => s.SystemRequirementsMin).Include(s => s.SystemRequirementsMax).
                Where(g => g.Name == dlcName && g.Type.ToLower() == "dlc").FirstOrDefault()!;
        }

        public IList<Game> GetDLCs()
        {
            return dataContext.Games.Where(d => d.Type == "DLC").Select(g => new Game
            {
                Id = g.Id,
                Name = g.Name,
                ReleaseDate = g.ReleaseDate,
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

        public bool DLCDelete(DLC dlc)
        {
            dataContext.Remove(dlc);
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
