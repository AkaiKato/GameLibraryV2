using GameLibraryV2.Models.Common;

namespace GameLibraryV2.Interfaces
{
    public interface IPlatformRepository
    {
        public Task<Platform> GetPlatformByIdAsync(int platformId);

        public Task<Platform> GetPlatformByNameAsync(string name);

        public Task<IList<Platform>> GetPlatformsAsync();

        public Task<bool> PlatformExistAsync(int platformId);

        public Task<bool> PlatformNameAlredyInUseAsync(int platformId, string platformName);

        public void CreatePlatform(Platform platform);

        public void UpdatePlatfrom(Platform platform);

        public void DeletePlatform(Platform platform);

        public Task SavePlatformAsync();
    }
}
