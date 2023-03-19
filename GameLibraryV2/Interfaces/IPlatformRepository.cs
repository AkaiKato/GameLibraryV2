using GameLibraryV2.Models;

namespace GameLibraryV2.Interfaces
{
    public interface IPlatformRepository
    {
        public Platform GetPlatformById(int platformId);

        public Platform GetPlatformByName(string name);

        public IList<Platform> GetPlatforms();

        public IList<Game> GetGameByPlatform(int platformId);

        public bool PlatformExist(int platformId);
    }
}
