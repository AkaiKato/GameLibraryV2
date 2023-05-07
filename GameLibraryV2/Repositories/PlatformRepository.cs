using GameLibraryV2.Data;
using GameLibraryV2.Interfaces;
using GameLibraryV2.Models.Common;
using Microsoft.EntityFrameworkCore;

namespace GameLibraryV2.Repositories
{
    public class PlatformRepository : IPlatformRepository
    {
        private readonly DataContext dataContext;

        public PlatformRepository(DataContext context)
        {
            dataContext = context;
        }

        public async Task<Platform> GetPlatformByIdAsync(int platformId)
        {
            return await dataContext.Platforms.FirstOrDefaultAsync(p => p.Id == platformId)!;
        }

        public async Task<Platform> GetPlatformByNameAsync(string name)
        {
            return await dataContext.Platforms.FirstOrDefaultAsync(p => p.Name.Trim().ToLower() == name.Trim().ToLower())!;
        }

        public async Task<IList<Platform>> GetPlatformsAsync()
        {
            return await dataContext.Platforms.OrderBy(p => p.Id).ToListAsync();
        }

        public async Task<bool> PlatformExistAsync(int platformId)
        {
            return await dataContext.Platforms.AnyAsync(p => p.Id == platformId);
        }

        public async Task<bool> PlatformNameAlredyInUseAsync(int platformId, string platformName)
        {
            return await dataContext.Platforms.AnyAsync(p => p.Name.Trim().ToLower() == platformName.Trim().ToLower() && p.Id == platformId);
        }

        //--------------------------------------------

        public void CreatePlatform(Platform platform)
        {
            dataContext.Add(platform);
        }

        public void UpdatePlatfrom(Platform platform)
        {
            dataContext.Update(platform);
        }

        public void DeletePlatform(Platform platform)
        {
            dataContext.Remove(platform);
        }

        public async Task SavePlatformAsync()
        {
            await dataContext.SaveChangesAsync();
        }
    }
}
