using GameLibraryV2.Dto.create;
using GameLibraryV2.Dto.Statistic;
using GameLibraryV2.Dto.Update;
using GameLibraryV2.Helper;
using GameLibraryV2.Models.Common;

namespace GameLibraryTest
{
    public class Helper
    {
        readonly Random rand = new();
        
        public User CreateRandomUser()
        {
            return new User
            {
                Id = rand.Next(100),
                Email = RandomString(50),
                Nickname = RandomString(50),
                Password = RandomString(50),
                Age = rand.Next(15, 100),
                Gender = rand.Next(0, 1) == 0 ? "male" : "female",
                PicturePath = RandomString(50),
                RegistrationdDate = DateOnly.FromDateTime(DateTime.Now),
                UserRoles = new List<Role> { CreateRandomRole() },
                RefreshToken = RandomString(50),
                TokenCreated = DateTime.Now,
                TokenExpires = DateTime.Now,
            };
        }

        public Role CreateRandomRole()
        {
            return new Role
            {
                Id = rand.Next(100),
                RoleName = RandomString(50),
            };
        }

        public Game CreateRandomGame()
        {
            return new Game
            {
                Id = rand.Next(100),
                Name = RandomString(50),
                PicturePath = RandomString(100),
                Status = rand.Next(0, 1) == 0 ? "released" : "announsed",
                ReleaseDate = DateOnly.FromDateTime(DateTime.Now),
                Description = RandomString(50),
                AgeRating = CreateRandomAgeRating(),
                NSFW = false,
                Type = rand.Next(0, 1) == 0 ? "game" : "dlc",
                Rating = CreateRandomRating(),
                Developers = new List<Developer>() { CreateRandomDeveloper() },
                Publishers = new List<Publisher>() { CreateRandomPublisher() },
                Platforms = new List<Platform> { CreateRandomPlatform() },
                Genres = new List<Genre> { CreateRandomGenre() },
                Tags = new List<Tag> { CreateRandomTag() },
            };
        }

        public PersonGame CreateRandomPersonGame()
        {
            return new PersonGame
            {
                Id = Guid.NewGuid(),
                User = CreateRandomUser(),
                Game = CreateRandomGame(),
                Score = rand.Next(1,10),
                List = rand.Next(0, 1) == 0 ? "planned" : "completed",
                Favourite = rand.Next(0, 1) != 0,
            };
        }

        public SystemRequirements CreateSystemRequirements()
        {
            return new SystemRequirements
            {
                Id = rand.Next(100),
                Type = rand.Next(0, 1) == 0 ? "min" : "max",
                OC = RandomString(50),
                Processor = RandomString(50),
                RAM = RandomString(50),
                VideoCard = RandomString(50),
                DirectX = RandomString(50),
                Ethernet = RandomString(50),
                HardDriveSpace = RandomString(50),
                Additional = RandomString(50),
            };
        }

        public SystemRequirementsCreateDto CreateSystemRequirementsCreateDto()
        {
            return new SystemRequirementsCreateDto
            {
                Type = rand.Next(0, 1) == 0 ? "min" : "max",
                OC = RandomString(50),
                Processor = RandomString(50),
                RAM = RandomString(50),
                VideoCard = RandomString(50),
                DirectX = RandomString(50),
                Ethernet = RandomString(50),
                HardDriveSpace = RandomString(50),
                Additional = RandomString(50),
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

        public Review CreateRandomReview()
        {
            return new()
            {
                Id = rand.Next(100),
                Game = CreateRandomGame(),
                User = CreateRandomUser(),
                Rating = rand.Next(1, 10),
                Text = RandomString(50),
                PublishDate = DateOnly.FromDateTime(DateTime.UtcNow),
                ReviewRating = rand.Next(),
            };
        }

        public Friend CreateRandomFriend()
        {
            return new()
            {
                Id = rand.Next(100),
                User = CreateRandomUser(),
                Friendu = CreateRandomUser(),
            };
        }

        public DLC CreateRandomDlc()
        {
            return new()
            {
                Id = rand.Next(100),
                ParentGame = CreateRandomGame(),
                DLCGame = CreateRandomGame(),
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

        public CountStatistic CreateRandomCountStatistic()
        {
            return new()
            {
                Id = rand.Next(100),
                Name = RandomString(20),
                Count = rand.Next(100),
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
