using GameLibraryV2.Data;
using GameLibraryV2.Models;

namespace GameLibraryV2
{
    public class Seed
    {
        private readonly DataContext dataContext;

        public Seed(DataContext Context)
        {
            dataContext = Context;
        }

        public void SeedDataContext()
        {
            if (!dataContext.Games.Any())
            {
                //roles
                var admin = new Role()
                {
                    RoleName = "admin",
                };
                var userRole = new Role()
                {
                    RoleName = "user",
                };
                //-----------------------------------------------------
                //Library
                var rLibrary = new Library() { };
                var vLibrary = new Library() { };
                var tLibrary = new Library() { };
                //-----------------------------------------------------
                //users
                //Rail
                var userRail = new User()
                {
                    Email = "rail201@yandex.ru",
                    Password = "password",
                    Nickname = "SunPatterns",
                    Age = 21,
                    Gender = "Male",
                    PicturePath = $"{Directory.GetCurrentDirectory()}/uploads/userPicture/SunPatterns.jpg",
                    RegistrationdDate = DateTime.Now,
                    Library = rLibrary,
                    UserRoles = new List<Role>() { admin},
                };
                //Valera
                var uservalera = new User()
                {
                    Email = "ma6upa@yandex.ru",
                    Password = "password",
                    Nickname = "Ma6upa",
                    Age = 21,
                    Gender = "Male",
                    PicturePath = $"{Directory.GetCurrentDirectory()}/uploads/userPicture/ma6upa.jpg",
                    RegistrationdDate = DateTime.Now,
                    Library = vLibrary,
                    UserRoles = new List<Role>() { userRole}
                };
                //ThirdMan
                var userThird = new User()
                {
                    Email = "third@yandex.ru",
                    Password = "password",
                    Nickname = "third",
                    Age = 21,
                    Gender = "Male",
                    PicturePath = $"{Directory.GetCurrentDirectory()}/uploads/userPicture/ma6upa.jpg",
                    RegistrationdDate = DateTime.Now,
                    Library = tLibrary,
                    UserRoles = new List<Role>() { userRole},
                };
                //-----------------------------------------------------
                //Friends
                userRail.UserFriends = new List<Friend>()
                {
                   new Friend()
                   {
                       User = userRail,
                       Friendu = uservalera,
                   },
                   new Friend()
                   {
                       User = userRail,
                       Friendu = userThird,
                   },
                };
                uservalera.UserFriends = new List<Friend>()
                {
                    new Friend
                    {
                        User = uservalera,
                        Friendu = userRail,
                    },
                };
                userThird.UserFriends = new List<Friend>()
                {
                   new Friend()
                   {
                        User = userThird,
                        Friendu = uservalera,
                   },
                };

                //-----------------------------------------------------
                //Developers min 5
                var teamCherryDev = new Developer()
                {
                    Name = "Team Cherry",
                    Description = "Team Cherry is a small indie games team in Adelaide, South Australia. Our mission is to build crazy and exciting worlds for you to explore and conquer",
                    PicturePath = $"{Directory.GetCurrentDirectory()}/uploads/developerPicture/TeamCherry.jpg",
                    MiniPicturePath = $"{Directory.GetCurrentDirectory()}/uploads/developerMiniPicture/miniTeamCherry.jpg",
                };
                var braceYourSelfGamesDev = new Developer()
                {
                    Name = "Brace Yourself Games",
                    Description = "indie Game company",
                    PicturePath = $"{Directory.GetCurrentDirectory()}/uploads/developerPicture/BraceYourselfGames.jpg",
                    MiniPicturePath = $"{Directory.GetCurrentDirectory()}/uploads/developerMiniPicture/miniBraceYourselfGames.jpg",
                };
                var kleiEntertaimentDev = new Developer()
                {
                    Name = "Klei Entertaiment",
                    Description = "Cool dev",
                    PicturePath = $"{Directory.GetCurrentDirectory()}/uploads/developerPicture/KleiEntertaiment.jpg",
                    MiniPicturePath = $"{Directory.GetCurrentDirectory()}/uploads/developerMiniPicture/miniKleiEntertaiment.jpg",
                };
                var dodgeRollDev = new Developer()
                {
                    Name = "Dodge Roll",
                    Description = "developers of gangeon series",
                    PicturePath = $"{Directory.GetCurrentDirectory()}/uploads/developerPicture/DodgeRoll.jpg",
                    MiniPicturePath = $"{Directory.GetCurrentDirectory()}/uploads/developerMiniPicture/miniDodgeRoll.jpg",
                };
                var massiveMonsterDev = new Developer()
                {
                    Name = "Massive Monster",
                    Description = "Developers of cult of the lamb!",
                    PicturePath = $"{Directory.GetCurrentDirectory()}/uploads/developerPicture/MassiveMonster.jpg",
                    MiniPicturePath = $"{Directory.GetCurrentDirectory()}/uploads/developerMiniPicture/miniMassiveMonster.jpg",
                };
                var scsSoftwareDev = new Developer()
                {
                    Name = "SCS Software",
                    Description = "Best simulator developer",
                    PicturePath = $"{Directory.GetCurrentDirectory()}/uploads/developerPicture/SCSSoftware.jpg",
                    MiniPicturePath = $"{Directory.GetCurrentDirectory()}/uploads/developerMiniPicture/miniSCSSoftware.jpg",
                };
                //-----------------------------------------------------
                //Publishers min 5
                var teamCherryPub = new Publisher()
                {
                    Name = teamCherryDev.Name,
                    Description = teamCherryDev.Description,
                    PicturePath = $"{Directory.GetCurrentDirectory()}/uploads/publisherPicture/TeamCherry.jpg",
                    MiniPicturePath = $"{Directory.GetCurrentDirectory()}/uploads/publisherMiniPicture/miniTeamCherry.jpg",
                };
                var braceYourSelfGamesPub = new Publisher()
                {
                    Name = braceYourSelfGamesDev.Name,
                    Description = braceYourSelfGamesDev.Description,
                    PicturePath = $"{Directory.GetCurrentDirectory()}/uploads/publisherPicture/BraceYourSelfGames.jpg",
                    MiniPicturePath = $"{Directory.GetCurrentDirectory()}/uploads/publisherMiniPicture/miniBraceYourSelfGames.jpg",
                };
                var kleiEntertaimentPub = new Publisher()
                {
                    Name = "Klei Entertaiment",
                    Description = "Cool publisher",
                    PicturePath = $"{Directory.GetCurrentDirectory()}/uploads/publisherPicture/KleiEntertaiment.jpg",
                    MiniPicturePath = $"{Directory.GetCurrentDirectory()}/uploads/publisherMiniPicture/miniKleiEntertaiment.jpg",
                };
                var devolverDigitalPub = new Publisher()
                {
                    Name = "Devolver Digital",
                    Description = "BEST PUBLISHER",
                    PicturePath = $"{Directory.GetCurrentDirectory()}/uploads/publisherPicture/DevolverDigital.jpg",
                    MiniPicturePath = $"{Directory.GetCurrentDirectory()}/uploads/publisherMiniPicture/miniDevolverDigital.jpg",
                };
                var scsSoftwarePub = new Publisher()
                {
                    Name = scsSoftwareDev.Name,
                    Description = scsSoftwareDev.Description,
                    PicturePath = $"{Directory.GetCurrentDirectory()}/uploads/publisherPicture/SCSSoftware.jpg",
                    MiniPicturePath = $"{Directory.GetCurrentDirectory()}/uploads/publisherMiniPicture/miniSCSSoftware.jpg",
                };
                //------------------------------------------------------
                //Genres
                var action = new Genre()
                {
                    Name = "Action",
                    Description = "action - desc",
                };
                var strategy = new Genre()
                {
                    Name = "Strategy",
                    Description = "strategy - desc",
                };
                var rpg = new Genre()
                {
                    Name = "RPG",
                    Description = "RPG - desc",
                };
                var adventure = new Genre()
                {
                    Name = "Adventure",
                    Description = "adventure - desc",
                };
                var puzzle = new Genre()
                {
                    Name = "Puzzle",
                    Description = "puzzle - desc",
                };
                var racing = new Genre()
                {
                    Name = "Racing",
                    Description = "racing - desc",
                };
                var sport = new Genre()
                {
                    Name = "Sport",
                    Description = "sport - desc",
                };
                var simulator = new Genre()
                {
                    Name = "Simulator",
                    Description = "Simulator -desc"
                };
                //------------------------------------------------------
                //Platforms
                var windows = new Platform()
                {
                    Name = "Windows",
                    Description = "Windows PC",
                };
                var linux = new Platform()
                {
                    Name = "Linux",
                    Description = "Linux PC",
                };
                var macos = new Platform()
                {
                    Name = "macOS",
                    Description = "macOC PC",
                };
                var playstation5 = new Platform()
                {
                    Name = "Playstation 5",
                    Description = "PS",
                };
                var playstation4 = new Platform()
                {
                    Name = "Playstation 4",
                    Description = "PS",
                };
                var xboxseriesx = new Platform()
                {
                    Name = "XBOX Series X",
                    Description = "PS",
                };
                var xboxone = new Platform()
                {
                    Name = "XBOX One",
                    Description = "PS",
                };
                var nintendo = new Platform()
                {
                    Name = "Nintendo",
                };
                //------------------------------------------------------
                //Tags
                var metroidvania = new Tag()
                {
                    Name = "Metroidvania",
                };
                var platformer = new Tag()
                {
                    Name = "Metroidvania",
                };
                var secondd = new Tag()
                {
                    Name = "2D",
                };
                var hard = new Tag()
                {
                    Name = "Hard",
                };
                var indie = new Tag()
                {
                    Name = "Indie",
                };
                var sideView = new Tag()
                {
                    Name = "Side View",
                };
                var roguelike = new Tag()
                {
                    Name = "Rogue-Like",
                };
                var rythm = new Tag()
                {
                    Name = "Rythm",
                };
                var shooter = new Tag()
                {
                    Name = "Shooter",
                };
                var twinstick = new Tag()
                {
                    Name = "Twin Stick",
                };
                var singleplayer = new Tag()
                {
                    Name = "Singleplayer",
                };
                var real = new Tag()
                {
                    Name = "Realizm",
                };
                var economic = new Tag()
                {
                    Name = "Economic",
                };
                var casual = new Tag()
                {
                    Name = "Casual",
                };
                var relax = new Tag()
                {
                    Name = "relax",
                };
                //-------------------------------------------------------
                //Game

                //EuroTruck
                var euroTruck = new Game()
                {
                    Name = "Euro Truck Simulator 2",
                    PicturePath = $"{Directory.GetCurrentDirectory()}/uploads/gamePicture/EuroTruck.jpg",
                    ReleaseDate = new DateTime(2012, 10, 18),
                    Description = "Станьте королем европейских дорог — водителем грузовика, который доставляет важные грузы на немалые расстояния! Вас ждут десятки городов Великобритании, Бельгии, Германии, Италии, Нидерландов, Польши и не только. Испытайте свои умения, выносливость и скорость.",
                    AgeRating = "Everyone",
                    AveragePlayTime = "44.5",
                    NSFW = false,
                    Type = "Game",
                    SystemRequirementsMin = new SystemRequirementsMin
                    {
                        OC = "Windows 7",
                        Processor = "Dual core CPU 2.4 GHz",
                        RAM = "4 GB RAM",
                        VideoCard = "GeForce GTS 450-class (Intel HD 4000)",
                        HardDriveSpace = "12 GB available space",
                    },
                    SystemRequirementsMax = new SystemRequirementsMax
                    {
                        OC = "Windows 7/8.1/10 64-bit",
                        Processor = "Quad core CPU 3.0 GHz",
                        RAM = "6 GB RAM",
                        VideoCard = "GeForce GTX 760-class (2 GB)",
                        HardDriveSpace = "12 GB available space",
                    },
                    Rating = new Rating
                    {
                        TotalRating = 7.69,
                        NumberOfOne = 1,
                        NumberOfTwo = 1,
                        NumberOfTen = 10,
                        NumberOfEight = 5,
                        NumberOfSeven = 7,
                    },
                    Developers = new List<Developer> { scsSoftwareDev },
                    Publishers = new List<Publisher> { scsSoftwarePub },
                    Platforms = new List<Platform> { windows, macos, linux },
                    Genres = new List<Genre> { simulator },
                    Tags = new List<Tag> { indie, singleplayer, relax, casual, economic, real },
                };
                var euroTruckGoingEast = new Game()
                {
                    Name = "Euro Truck Simulator 2 - Going East",
                    PicturePath = $"{Directory.GetCurrentDirectory()}/uploads/gamePicture/EuroTruckGoingEast!.jpg",
                    ReleaseDate = new DateTime(2013, 09, 20),
                    Description = "Станьте королем европейских дорог — водителем грузовика, который доставляет важные грузы на немалые расстояния! Вас ждут десятки городов Великобритании, Бельгии, Германии, Италии, Нидерландов, Польши и не только. Испытайте свои умения, выносливость и скорость.",
                    AgeRating = "Everyone",
                    AveragePlayTime = "44.5",
                    NSFW = false,
                    Type = "DLC",
                    ParentGame = euroTruck,
                    SystemRequirementsMin = new SystemRequirementsMin
                    {
                        OC = "Windows 7",
                        Processor = "Dual core CPU 2.4 GHz",
                        RAM = "4 GB RAM",
                        VideoCard = "GeForce GTS 450-class (Intel HD 4000)",
                        HardDriveSpace = "50 MB",
                    },
                    SystemRequirementsMax = new SystemRequirementsMax
                    {
                        OC = "Windows 7/8.1/10 64-bit",
                        Processor = "Quad core CPU 3.0 GHz",
                        RAM = "6 GB RAM",
                        VideoCard = "GeForce GTX 760-class (2 GB)",
                        HardDriveSpace = "50 MB",
                    },
                    Rating = new Rating
                    {
                        TotalRating = 6.69,
                        NumberOfOne = 1,
                        NumberOfTwo = 1,
                        NumberOfTen = 10,
                        NumberOfEight = 5,
                        NumberOfSeven = 7,
                    },
                    Developers = new List<Developer> { scsSoftwareDev },
                    Publishers = new List<Publisher> { scsSoftwarePub },
                    Platforms = new List<Platform> { windows, macos, linux },
                    Genres = new List<Genre> { simulator },
                    Tags = new List<Tag> { indie, singleplayer, relax, casual, economic, real },
                };
                var euroTruckScandinavia = new Game()
                {
                    Name = "Euro Truck Simulator 2 - Scandinavia",
                    PicturePath = $"{Directory.GetCurrentDirectory()}/uploads/gamePicture/EuroTruckScandinavia!.jpg",
                    ReleaseDate = new DateTime(2015, 05, 6),
                    Description = "Станьте королем европейских дорог — водителем грузовика, который доставляет важные грузы на немалые расстояния! Вас ждут десятки городов Великобритании, Бельгии, Германии, Италии, Нидерландов, Польши и не только. Испытайте свои умения, выносливость и скорость.",
                    AgeRating = "Everyone",
                    AveragePlayTime = "44.5",
                    NSFW = false,
                    Type = "DLC",
                    ParentGame = euroTruck,
                    SystemRequirementsMin = new SystemRequirementsMin
                    {
                        OC = "Windows 7",
                        Processor = "Dual core CPU 2.4 GHz",
                        RAM = "4 GB RAM",
                        VideoCard = "GeForce GTS 450-class (Intel HD 4000)",
                        HardDriveSpace = "200 MB",
                    },
                    SystemRequirementsMax = new SystemRequirementsMax
                    {
                        OC = "Windows 7/8.1/10 64-bit",
                        Processor = "Quad core CPU 3.0 GHz",
                        RAM = "6 GB RAM",
                        VideoCard = "GeForce GTX 760-class (2 GB)",
                        HardDriveSpace = "200 MB",
                    },
                    Rating = new Rating
                    {
                        TotalRating = 6.69,
                        NumberOfOne = 1,
                        NumberOfTwo = 1,
                        NumberOfTen = 10,
                        NumberOfEight = 5,
                        NumberOfSeven = 7,
                    },
                    Developers = new List<Developer> { scsSoftwareDev },
                    Publishers = new List<Publisher> { scsSoftwarePub },
                    Platforms = new List<Platform> { windows, macos, linux },
                    Genres = new List<Genre> { simulator },
                    Tags = new List<Tag> { indie, singleplayer, relax, casual, economic, real },
                };
                var euroTruckHighPowerCargoPack = new Game()
                {
                    Name = "Euro Truck Simulator 2 - High Power Cargo Pack",
                    PicturePath = $"{Directory.GetCurrentDirectory()}/uploads/gamePicture/HighPowerCargoPack.jpg",
                    ReleaseDate = new DateTime(2014, 08, 14),
                    Description = "Станьте королем европейских дорог — водителем грузовика, который доставляет важные грузы на немалые расстояния! Вас ждут десятки городов Великобритании, Бельгии, Германии, Италии, Нидерландов, Польши и не только. Испытайте свои умения, выносливость и скорость.",
                    AgeRating = "Everyone",
                    AveragePlayTime = "44.5",
                    NSFW = false,
                    Type = "DLC",
                    ParentGame = euroTruck,
                    SystemRequirementsMin = new SystemRequirementsMin
                    {
                        OC = "Windows 7",
                        Processor = "Dual core CPU 2.4 GHz",
                        RAM = "4 GB RAM",
                        VideoCard = "GeForce GTS 450-class (Intel HD 4000)",
                        HardDriveSpace = "20 MB",
                    },
                    SystemRequirementsMax = new SystemRequirementsMax
                    {
                        OC = "Windows 7/8.1/10 64-bit",
                        Processor = "Quad core CPU 3.0 GHz",
                        RAM = "6 GB RAM",
                        VideoCard = "GeForce GTX 760-class (2 GB)",
                        HardDriveSpace = "20 MB",
                    },
                    Rating = new Rating
                    {
                        TotalRating = 6.69,
                        NumberOfOne = 1,
                        NumberOfTwo = 1,
                        NumberOfTen = 10,
                        NumberOfEight = 5,
                        NumberOfSeven = 7,
                    },
                    Developers = new List<Developer> { scsSoftwareDev },
                    Publishers = new List<Publisher> { scsSoftwarePub },
                    Platforms = new List<Platform> { windows, macos, linux },
                    Genres = new List<Genre> { simulator },
                    Tags = new List<Tag> { indie, singleplayer, relax, casual, economic, real },
                };
                var dlcEuro1 = new DLC
                {
                    ParentGame = euroTruck,
                    DLCGame = euroTruckGoingEast,
                };
                var dlcEuro2 = new DLC
                {
                    ParentGame = euroTruck,
                    DLCGame = euroTruckScandinavia,
                };
                var dlcEuro3 = new DLC
                {
                    ParentGame = euroTruck,
                    DLCGame = euroTruckHighPowerCargoPack,
                };
                euroTruck.DLCs = new List<DLC> { dlcEuro1, dlcEuro2, dlcEuro3 };
                euroTruck.Reviews = new List<Review>()
                {
                    new Review
                    {
                        Game = euroTruck,
                        User = userRail,
                        Rating = 10,
                        Text = "Best way to live life",
                        PublishDate = DateTime.Now,
                        ReviewRating = 500,
                    },
                    new Review
                    {
                        Game = euroTruck,
                        User = userThird,
                        Rating = 1,
                        Text = "Worse way to live life",
                        PublishDate = DateTime.Now,
                        ReviewRating = -100,
                    },
                };
                //---------
                //HK
                var hollowKnight = new Game()
                {
                    Name = "Hollow Knight",
                    PicturePath = $"{Directory.GetCurrentDirectory()}/uploads/gamePicture/HollowKnight.jpg",
                    ReleaseDate = new DateTime(2017, 02, 24),
                    Description = "Hollow Knight – это эпическое приключение в огромном разрушенном королевстве, полном насекомых и героев. Исследуйте извилистые пещеры, сражайтесь с порчеными тварями и заводите дружбу со странными жуками – все это в классической двухмерной ручной рисовке.",
                    AgeRating = "Everyone",
                    AveragePlayTime = "26.5",
                    NSFW = false,
                    Type = "Game",
                    SystemRequirementsMin = new SystemRequirementsMin
                    {
                        OC = "Windows 7 (64bit)",
                        Processor = "Intel Core 2 Duo E5200",
                        RAM = "4 GB RAM",
                        VideoCard = "GeForce 9800GTX+ (1GB)",
                        DirectX = "Version 10",
                        HardDriveSpace = "9 GB",
                        Additional = "1080p, 16:9 recommended",
                    },
                    SystemRequirementsMax = new SystemRequirementsMax
                    {
                        OC = "Windows 10 (64bit)",
                        Processor = "Intel Core i5",
                        RAM = "8 GB RAM",
                        VideoCard = "GeForce GTX 560",
                        DirectX = "Version 11",
                        HardDriveSpace = "9 GB",
                        Additional = "1080p, 16:9 recommended",
                    },
                    Rating = new Rating
                    {
                        TotalRating = 9.69,
                        NumberOfOne = 1,
                        NumberOfTwo = 1,
                        NumberOfTen = 10,
                        NumberOfEight = 5,
                        NumberOfSeven = 7,
                    },
                    Developers = new List<Developer> { teamCherryDev },
                    Publishers = new List<Publisher> { teamCherryPub },
                    Platforms = new List<Platform> { windows, macos, linux, playstation4, xboxone, nintendo },
                    Genres = new List<Genre> { action },
                    Tags = new List<Tag> { indie, singleplayer, metroidvania, platformer, secondd, hard },
                };
                var hollowKnightHiddenDreams = new Game()
                {
                    Name = "Hollow Knight: HiddenDreams",
                    PicturePath = $"{Directory.GetCurrentDirectory()}/uploads/gamePicture/HollowKnightHiddenDreams.jpg",
                    ReleaseDate = new DateTime(2017, 06, 03),
                    Description = "Hollow Knight – это эпическое приключение в огромном разрушенном королевстве, полном насекомых и героев. Исследуйте извилистые пещеры, сражайтесь с порчеными тварями и заводите дружбу со странными жуками – все это в классической двухмерной ручной рисовке.",
                    AgeRating = "Everyone",
                    AveragePlayTime = "30 mins",
                    NSFW = false,
                    Type = "DLC",
                    ParentGame = hollowKnight,
                    SystemRequirementsMin = new SystemRequirementsMin
                    {
                        OC = "Windows 7 (64bit)",
                        Processor = "Intel Core 2 Duo E5200",
                        RAM = "4 GB RAM",
                        VideoCard = "GeForce 9800GTX+ (1GB)",
                        DirectX = "Version 10",
                        HardDriveSpace = "9 GB",
                        Additional = "1080p, 16:9 recommended",
                    },
                    SystemRequirementsMax = new SystemRequirementsMax
                    {
                        OC = "Windows 10 (64bit)",
                        Processor = "Intel Core i5",
                        RAM = "8 GB RAM",
                        VideoCard = "GeForce GTX 560",
                        DirectX = "Version 11",
                        HardDriveSpace = "9 GB",
                        Additional = "1080p, 16:9 recommended",
                    },
                    Rating = new Rating
                    {
                        TotalRating = 9.69,
                        NumberOfOne = 1,
                        NumberOfTwo = 1,
                        NumberOfTen = 10,
                        NumberOfEight = 5,
                        NumberOfSeven = 7,
                    },
                    Developers = new List<Developer> { teamCherryDev },
                    Publishers = new List<Publisher> { teamCherryPub },
                    Platforms = new List<Platform> { windows, macos, linux, playstation4, xboxone, nintendo },
                    Genres = new List<Genre> { action },
                    Tags = new List<Tag> { indie, singleplayer, metroidvania, platformer, secondd, hard },
                };
                var hollowKnightTheGrimmTroupe = new Game()
                {
                    Name = "Hollow Knight: The Grimm Troupe",
                    PicturePath = $"{Directory.GetCurrentDirectory()}/uploads/gamePicture/HollowKnightTheGrimmTroupe.jpg",
                    ReleaseDate = new DateTime(2017, 10, 26),
                    Description = "Hollow Knight – это эпическое приключение в огромном разрушенном королевстве, полном насекомых и героев. Исследуйте извилистые пещеры, сражайтесь с порчеными тварями и заводите дружбу со странными жуками – все это в классической двухмерной ручной рисовке.",
                    AgeRating = "Everyone",
                    AveragePlayTime = "2",
                    NSFW = false,
                    Type = "DLC",
                    ParentGame = hollowKnight,
                    SystemRequirementsMin = new SystemRequirementsMin
                    {
                        OC = "Windows 7 (64bit)",
                        Processor = "Intel Core 2 Duo E5200",
                        RAM = "4 GB RAM",
                        VideoCard = "GeForce 9800GTX+ (1GB)",
                        DirectX = "Version 10",
                        HardDriveSpace = "9 GB",
                        Additional = "1080p, 16:9 recommended",
                    },
                    SystemRequirementsMax = new SystemRequirementsMax
                    {
                        OC = "Windows 10 (64bit)",
                        Processor = "Intel Core i5",
                        RAM = "8 GB RAM",
                        VideoCard = "GeForce GTX 560",
                        DirectX = "Version 11",
                        HardDriveSpace = "9 GB",
                        Additional = "1080p, 16:9 recommended",
                    },
                    Rating = new Rating
                    {
                        TotalRating = 9.69,
                        NumberOfOne = 1,
                        NumberOfTwo = 1,
                        NumberOfTen = 10,
                        NumberOfEight = 5,
                        NumberOfSeven = 7,
                    },
                    Developers = new List<Developer> { teamCherryDev },
                    Publishers = new List<Publisher> { teamCherryPub },
                    Platforms = new List<Platform> { windows, macos, linux, playstation4, xboxone, nintendo },
                    Genres = new List<Genre> { action },
                    Tags = new List<Tag> { indie, singleplayer, metroidvania, platformer, secondd, hard },
                };
                var hollowKnightGodmaster = new Game()
                {
                    Name = "Hollow Knight: Godmaster",
                    PicturePath = $"{Directory.GetCurrentDirectory()}/uploads/gamePicture/HollowKnightGodmaster.jpg",
                    ReleaseDate = new DateTime(2018, 06, 23),
                    Description = "Hollow Knight – это эпическое приключение в огромном разрушенном королевстве, полном насекомых и героев. Исследуйте извилистые пещеры, сражайтесь с порчеными тварями и заводите дружбу со странными жуками – все это в классической двухмерной ручной рисовке.",
                    AgeRating = "Everyone",
                    AveragePlayTime = "26.5",
                    NSFW = false,
                    Type = "DLC",
                    ParentGame = hollowKnight,
                    SystemRequirementsMin = new SystemRequirementsMin
                    {
                        OC = "Windows 7 (64bit)",
                        Processor = "Intel Core 2 Duo E5200",
                        RAM = "4 GB RAM",
                        VideoCard = "GeForce 9800GTX+ (1GB)",
                        DirectX = "Version 10",
                        HardDriveSpace = "9 GB",
                        Additional = "1080p, 16:9 recommended",
                    },
                    SystemRequirementsMax = new SystemRequirementsMax
                    {
                        OC = "Windows 10 (64bit)",
                        Processor = "Intel Core i5",
                        RAM = "8 GB RAM",
                        VideoCard = "GeForce GTX 560",
                        DirectX = "Version 11",
                        HardDriveSpace = "9 GB",
                        Additional = "1080p, 16:9 recommended",
                    },
                    Rating = new Rating
                    {
                        TotalRating = 9.69,
                        NumberOfOne = 1,
                        NumberOfTwo = 1,
                        NumberOfTen = 10,
                        NumberOfEight = 5,
                        NumberOfSeven = 7,
                    },
                    Developers = new List<Developer> { teamCherryDev },
                    Publishers = new List<Publisher> { teamCherryPub },
                    Platforms = new List<Platform> { windows, macos, linux, playstation4, xboxone, nintendo },
                    Genres = new List<Genre> { action },
                    Tags = new List<Tag> { indie, singleplayer, metroidvania, platformer, secondd, hard },
                };
                var dlcHollow1 = new DLC
                {
                    ParentGame = hollowKnight,
                    DLCGame = hollowKnightHiddenDreams,
                };
                var dlcHollow2 = new DLC
                {
                    ParentGame = hollowKnight,
                    DLCGame = hollowKnightTheGrimmTroupe,
                };
                var dlcHollow3 = new DLC
                {
                    ParentGame = hollowKnight,
                    DLCGame = hollowKnightGodmaster,
                };
                hollowKnight.DLCs = new List<DLC> { dlcHollow1, dlcHollow2, dlcHollow3 };
                hollowKnight.Reviews = new List<Review>()
                {
                    new Review
                    {
                        Game = hollowKnight,
                        User = userRail,
                        Rating = 10,
                        Text = "Best",
                        PublishDate = DateTime.Now,
                        ReviewRating = 100,
                    },
                    new Review
                    {
                        Game = hollowKnight,
                        User = uservalera,
                        Rating = 10,
                        Text = "Best",
                        PublishDate = DateTime.Now,
                        ReviewRating = 100,
                    },
                    new Review
                    {
                        Game = hollowKnight,
                        User = userThird,
                        Rating = 10,
                        Text = "Best",
                        PublishDate = DateTime.Now,
                        ReviewRating = 100,
                    },
                };
                var hungryKnight = new Game()
                {
                    Name = "Hungry Knight",
                    PicturePath = $"{Directory.GetCurrentDirectory()}/uploads/gamePicture/HungryKnight.jpg",
                    ReleaseDate = new DateTime(2015, 02, 24),
                    Description = "A challenging action game. It's hard, but you can do it! Be brave.",
                    AgeRating = "Everyone",
                    AveragePlayTime = "1",
                    NSFW = false,
                    Type = "Game",
                    SystemRequirementsMin = new SystemRequirementsMin
                    {
                        OC = "Windows 7 (64bit)",
                    },
                    SystemRequirementsMax = new SystemRequirementsMax
                    {
                        OC = "Windows 10 (64bit)",
                    },
                    Rating = new Rating
                    {
                        TotalRating = 5.69,
                        NumberOfOne = 1,
                        NumberOfTwo = 1,
                        NumberOfTen = 10,
                        NumberOfEight = 5,
                        NumberOfSeven = 7,
                    },
                    Developers = new List<Developer> { teamCherryDev },
                    Publishers = new List<Publisher> { teamCherryPub },
                    Platforms = new List<Platform> { windows },
                    Genres = new List<Genre> { action },
                    Tags = new List<Tag> { indie, singleplayer, hard },
                };
                //---------
                //Enter the dungeon
                var enterTheGungeon = new Game()
                {
                    Name = "Enter the Gungeon",
                    PicturePath = $"{Directory.GetCurrentDirectory()}/uploads/gamePicture/EntertheGungeon.jpg",
                    ReleaseDate = new DateTime(2016, 04, 5),
                    Description = "Enter the Gungeon - игра жанра “пулевая завеса в подземелье”, в которой ищущие спасения неудачники стреляют, грабят, перекатываются и опрокидывают столы, чтобы добраться до легендарного сокровища Оружелья: оружия, которое может убить прошлое.",
                    AgeRating = "Teen",
                    AveragePlayTime = "22.5",
                    NSFW = false,
                    Type = "Game",
                    SystemRequirementsMin = new SystemRequirementsMin
                    {
                        OC = "Windows 7 or later",
                        Processor = "Intel Core 2 Duo E6320 (2*1866) or equivalent",
                        RAM = "2 GB RAM",
                        VideoCard = "GeForce 7600 GS (512 MB) or equivalent",
                        HardDriveSpace = "2 GB",
                    },
                    SystemRequirementsMax = new SystemRequirementsMax
                    {
                        OC = "Windows 7 or later",
                        Processor = "Intel Core 2 Duo E6320 (2*1866) or equivalent",
                        RAM = "2 GB RAM",
                        VideoCard = "GeForce 7600 GS (512 MB) or equivalent",
                        HardDriveSpace = "2 GB",
                    },
                    Rating = new Rating
                    {
                        TotalRating = 8.69,
                        NumberOfOne = 1,
                        NumberOfTwo = 1,
                        NumberOfTen = 10,
                        NumberOfEight = 5,
                        NumberOfSeven = 7,
                    },
                    Developers = new List<Developer> { dodgeRollDev },
                    Publishers = new List<Publisher> { devolverDigitalPub },
                    Platforms = new List<Platform> { windows, macos, linux, playstation4, xboxone, nintendo },
                    Genres = new List<Genre> { action },
                    Tags = new List<Tag> { indie, singleplayer, hard, shooter, twinstick },
                };
                //---------
                //Cult of the lamb
                var cultOfTheLamb = new Game()
                {
                    Name = "Cult of the Lamb",
                    PicturePath = $"{Directory.GetCurrentDirectory()}/uploads/gamePicture/CultOfTheLamb.jpg",
                    ReleaseDate = new DateTime(2022, 08, 11),
                    Description = "Взращивайте собственный культ в землях лжепророков, совершайте походы по таинственным уголкам леса, объединяйте вокруг себя верных последователей и несите своё слово в массы, чтобы сделать свой культ единственным.",
                    AgeRating = "Teen",
                    AveragePlayTime = "14",
                    NSFW = false,
                    Type = "Game",
                    SystemRequirementsMin = new SystemRequirementsMin
                    {
                        OC = "Windows 7 or later",
                        Processor = "Intel Core i3-3240 (2 * 3400); AMD FX-4300 (4 * 3800)",
                        RAM = "4 GB RAM",
                        VideoCard = "GeForce GTX 560 Ti (1024 VRAM); Radeon HD 7750 (1024 VRAM)",
                        HardDriveSpace = "4 GB",
                    },
                    SystemRequirementsMax = new SystemRequirementsMax
                    {
                        OC = "Windows 10",
                        Processor = "Intel Core i5-3470",
                        RAM = "8 GB RAM",
                        VideoCard = "GeForce GTX 1050 (2048 VRAM); Radeon R9 380 (2048 VRAM)",
                        HardDriveSpace = "4 GB",
                    },
                    Rating = new Rating
                    {
                        TotalRating = 8.69,
                        NumberOfOne = 1,
                        NumberOfTwo = 1,
                        NumberOfTen = 10,
                        NumberOfEight = 5,
                        NumberOfSeven = 7,
                    },
                    Developers = new List<Developer> { massiveMonsterDev },
                    Publishers = new List<Publisher> { devolverDigitalPub },
                    Platforms = new List<Platform> { windows, macos, playstation4, playstation5, xboxone, xboxseriesx, nintendo },
                    Genres = new List<Genre> { action },
                    Tags = new List<Tag> { indie, singleplayer, secondd, roguelike, },
                };
                //---------
                //Don't Starve
                var dontStarveTogether = new Game()
                {
                    Name = "Don't Starve Together",
                    PicturePath = $"{Directory.GetCurrentDirectory()}/uploads/gamePicture/DontStarveTogether.jpg",
                    ReleaseDate = new DateTime(2016, 04, 21),
                    Description = "Fight, Farm, Build and Explore Together in the standalone multiplayer expansion to the uncompromising wilderness survival game, Don't Starve.",
                    AgeRating = "Teen",
                    AveragePlayTime = "35.5",
                    NSFW = false,
                    Type = "Game",
                    SystemRequirementsMin = new SystemRequirementsMin
                    {
                        OC = "Windows Vista/Windows 7/Windows 8",
                        Processor = "1.7+ GHz or better",
                        RAM = "1 GB RAM",
                        VideoCard = "Radeon HD5450 or better; 256 MB or higher",
                        DirectX = "Version 9.0c",
                        Ethernet = "connection to the Ethernet",
                        HardDriveSpace = "3 GB",
                        Additional = "100% DirectX9.0c compatible sound card and drivers",
                    },
                    SystemRequirementsMax = new SystemRequirementsMax
                    {
                        OC = "Windows Vista/Windows 7/Windows 8",
                        Processor = "1.7+ GHz or better",
                        RAM = "1 GB RAM",
                        VideoCard = "Radeon HD5450 or better; 256 MB or higher",
                        DirectX = "Version 9.0c",
                        Ethernet = "connection to the Ethernet",
                        HardDriveSpace = "3 GB",
                        Additional = "100% DirectX9.0c compatible sound card and drivers",
                    },
                    Rating = new Rating
                    {
                        TotalRating = 8.69,
                        NumberOfOne = 1,
                        NumberOfTwo = 1,
                        NumberOfTen = 10,
                        NumberOfEight = 5,
                        NumberOfSeven = 7,
                    },
                    Developers = new List<Developer> { kleiEntertaimentDev },
                    Publishers = new List<Publisher> { kleiEntertaimentPub },
                    Platforms = new List<Platform> { windows, macos, linux, playstation4, xboxone },
                    Genres = new List<Genre> { action },
                    Tags = new List<Tag> { indie, secondd, hard, sideView},
                };
                //---------
                //PersonGames
                rLibrary.PersonGames = new List<PersonGame>()
                {
                    new PersonGame
                    {
                        Game = hollowKnight,
                        Score = 10,
                        Comment = "BEST!",
                        List = "Completed",
                        PlayedPlatform = "PC",
                        Favourite = true,
                    },
                    new PersonGame
                    {
                        Game = euroTruck,
                        Score = 10,
                        Comment = "BEST!",
                        List = "Completed",
                        PlayedPlatform = "PC",
                        Favourite = false,
                    },
                };
                vLibrary.PersonGames = new List<PersonGame>()
                {
                    new PersonGame
                    {
                        Game = hollowKnight,
                        Score = 10,
                        Comment = "BEST!",
                        List = "Completed",
                        PlayedPlatform = "PC",
                        Favourite = true,
                    },
                };
                tLibrary.PersonGames = new List<PersonGame>()
                {
                    new PersonGame
                    {
                        Game = euroTruck,
                        Score = 1,
                        Comment = "WORST!",
                        List = "Completed",
                        PlayedPlatform = "PC",
                        Favourite = false,
                    },
                };

                dataContext.Developers.AddRange(teamCherryDev, braceYourSelfGamesDev, kleiEntertaimentDev, dodgeRollDev, massiveMonsterDev, scsSoftwareDev);

                dataContext.Publishers.AddRange(teamCherryPub, braceYourSelfGamesPub, kleiEntertaimentPub, devolverDigitalPub, scsSoftwarePub);

                dataContext.Genres.AddRange(action, strategy, rpg, adventure, puzzle, racing, sport);

                dataContext.Platforms.AddRange(windows, linux, macos, playstation4, playstation5, xboxone, xboxseriesx, nintendo);

                dataContext.Tags.AddRange(metroidvania, platformer, secondd, hard, indie, sideView, roguelike, rythm, shooter, twinstick, singleplayer, real, economic, casual, relax);

                //Euro truck, HK, Enter the dungeon, Crypt, Cult of the lamb, Don't Starve
                dataContext.Games.AddRange(euroTruck, hollowKnight, hungryKnight, enterTheGungeon, cultOfTheLamb, dontStarveTogether);

                dataContext.DLCs.AddRange(dlcEuro1, dlcEuro2, dlcEuro3, dlcHollow1, dlcHollow2, dlcHollow3);

                dataContext.Roles.AddRange(admin, userRole);

                dataContext.Libraries.AddRange(rLibrary, vLibrary, tLibrary);

                dataContext.Users.AddRange(userRail, uservalera, userThird);

                dataContext.SaveChanges();
            }
        }
    }
}
