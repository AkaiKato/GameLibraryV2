﻿using GameLibraryV2.Data;
using GameLibraryV2.Interfaces;
using GameLibraryV2.Models;
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

        public ICollection<Game> GetGames()
        {
            return dataContext.Games.Select(p => new Game
            {
                Id = p.Id,
                Name = p.Name,
                PicturePath = p.PicturePath,
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

            }).ToList();
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
            return dataContext.Games.Include(d => d.DLCs)!.ThenInclude(dg => dg.DLCGame).Include(d => d.Developers).
                Include(p => p.Publishers).Include(p => p.Platforms).Include(g => g.Genres).
                Include(t => t.Tags).Include(p => p.ParentGame).Include(s => s.SystemRequirementsMin).
                Include(s => s.SystemRequirementsMax).Include(r => r.Rating).Where(x => x.Id == gameId).FirstOrDefault()!;
        }

        public Game GetGameByName(string gameName)
        {
            return dataContext.Games.Include(d => d.DLCs)!.ThenInclude(dg => dg.DLCGame).Include(d => d.Developers).
                Include(p => p.Publishers).Include(p => p.Platforms).Include(g => g.Genres).
                Include(t => t.Tags).Include(p => p.ParentGame).Include(s => s.SystemRequirementsMin).
                Include(s => s.SystemRequirementsMax).Include(r => r.Rating).Where(x => x.Name.Trim().ToLower() == gameName.Trim().ToLower()).FirstOrDefault()!;
        }

        public Game GetDLCById(int gameId)
        {
            return dataContext.Games.Include(d => d.DLCs)!.ThenInclude(dg => dg.DLCGame).Include(d => d.Developers).
                Include(p => p.Publishers).Include(p => p.Platforms).Include(g => g.Genres).
                Include(t => t.Tags).Include(p => p.ParentGame).Include(s => s.SystemRequirementsMin).
                Include(s => s.SystemRequirementsMax).Include(r => r.Rating).
                Where(x => x.Id == gameId && x.Type.Trim().ToLower() == "dlc".Trim().ToLower()).FirstOrDefault()!;
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

        public IList<Game> GetGamesByDeveloper(int developerId)
        {
            return dataContext.Games.Where(g => g.Developers.Any(d => d.Id == developerId)).Select(g => new Game
            {
                Id = g.Id,
                Name = g.Name,
                PicturePath = g.PicturePath,
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

        public IList<Game> GetGamesByGenre(int genreId)
        {
            return dataContext.Games.Where(g => g.Genres.Any(g => g.Id == genreId)).Select(g => new Game
            {
                Id = g.Id,
                Name = g.Name,
                PicturePath = g.PicturePath,
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

        public IList<Game> GetGameByPlatform(int platformId)
        {
            return dataContext.Games.Where(p => p.Platforms.Any(p => p.Id == platformId)).Select(g => new Game
            {
                Id = g.Id,
                Name = g.Name,
                PicturePath = g.PicturePath,
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

        public IList<Game> GetGamesByPublisher(int publisherId)
        {
            return dataContext.Games.Where(g => g.Publishers.Any(p => p.Id == publisherId)).Select(g => new Game
            {
                Id = g.Id,
                Name = g.Name,
                PicturePath = g.PicturePath,
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

        public IList<Game> GetGamesByTag(int tagId)
        {
            return dataContext.Games.Where(g => g.Tags.Any(g => g.Id == tagId)).Select(g => new Game
            {
                Id = g.Id,
                Name = g.Name,
                PicturePath = g.PicturePath,
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