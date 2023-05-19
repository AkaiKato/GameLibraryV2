using GameLibraryV2.Dto.Update;
using GameLibraryV2.Helper;
using GameLibraryV2.Models.Common;

namespace GameLibraryTest
{
    public class Helper
    {
        readonly Random rand = new();
        public Game CreateRandomGame()
        {
            return new Game
            {
                Id = rand.Next(100),
                Name = RandomString(50),
                PicturePath = RandomString(100),
                Status = RandomString(50),
                ReleaseDate = DateTime.UtcNow,
                Description = RandomString(50),
                AgeRating = CreateRandomAgeRating(),
                NSFW = false,
                Type = RandomString(50),
                Rating = CreateRandomRating(),
                Developers = new List<Developer>() { CreateRandomDeveloper() },
                Publishers = new List<Publisher>() { CreateRandomPublisher() },
                Platforms = new List<Platform> { CreateRandomPlatform() },
                Genres = new List<Genre> { CreateRandomGenre() },
                Tags = new List<Tag> { CreateRandomTag() },
            };
        }

        public Developer CreateRandomDeveloper()
        {
            return new Developer
            {
                Id = rand.Next(100),
                Name = RandomString(50),
                PicturePath = RandomString(100),
                MiniPicturePath = RandomString(100),
            };
        }

        public Publisher CreateRandomPublisher()
        {
            return new Publisher
            {
                Id = rand.Next(100),
                Name = RandomString(50),
                PicturePath = RandomString(100),
                MiniPicturePath = RandomString(100),
            };
        }

        public Platform CreateRandomPlatform()
        {
            return new Platform
            {
                Id = rand.Next(100),
                Name = RandomString(50),
            };
        }

        public Genre CreateRandomGenre()
        {
            return new Genre
            {
                Id = rand.Next(100),
                Name = RandomString(50),
            };
        }

        public Tag CreateRandomTag()
        {
            return new Tag
            {
                Id = rand.Next(100),
                Name = RandomString(50),
            };
        }

        public Rating CreateRandomRating()
        {
            return new Rating()
            {
                Id = rand.Next(100),
                TotalRating = rand.NextDouble(),
                NumberOfOne = rand.Next(100),
                NumberOfTwo = rand.Next(100),
                NumberOfThree = rand.Next(100),
                NumberOfFour = rand.Next(100),
                NumberOfFive = rand.Next(100),
                NumberOfSix = rand.Next(100),
                NumberOfSeven = rand.Next(100),
                NumberOfEight = rand.Next(100),
                NumberOfNine = rand.Next(100),
                NumberOfTen = rand.Next(100),
            };
        }

        public AgeRating CreateRandomAgeRating()
        {
            return new()
            {
                Id = rand.Next(100),
                Name = RandomString(20),
                Description = RandomString(100),
            };
        }

        public CommonUpdate CreateRandomCommonUpdate()
        {
            return new()
            {
                Id = rand.Next(100),
                Name = RandomString(20),
                Description = RandomString(100),
            };
        }

        public string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[rand.Next(s.Length)]).ToArray());
        }

        public PagedList<Game> ReturnPagedList(List<Game> games)
        {
            return new(games, 0, 1, 1);
        }
    }
}
