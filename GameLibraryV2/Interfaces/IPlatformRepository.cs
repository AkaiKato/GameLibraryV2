using GameLibraryV2.Models.Common;

namespace GameLibraryV2.Interfaces
{
    public interface IPlatformRepository
    {
        public Platform GetPlatformById(int platformId);

        public Platform GetPlatformByName(string name);

        public IList<Platform> GetPlatforms();

        public bool PlatformExist(int platformId);

        public bool PlatformNameAlredyInUse(int platformId, string platformName);

        public bool CreatePlatform(Platform platform);

        public bool UpdatePlatfrom(Platform platform);

        public bool DeletePlatform(Platform platform);
    }
}
