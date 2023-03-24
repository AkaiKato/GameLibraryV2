using GameLibraryV2.Data;
using GameLibraryV2.Interfaces;
using GameLibraryV2.Models;

namespace GameLibraryV2.Repositories
{
    public class PlatformRepository : IPlatformRepository
    {
        private readonly DataContext dataContext;

        public PlatformRepository(DataContext context)
        {
            dataContext = context;
        }

        public Platform GetPlatformById(int platformId)
        {
            return dataContext.Platforms.Where(p => p.Id == platformId).FirstOrDefault()!;
        }

        public Platform GetPlatformByName(string name)
        {
            return dataContext.Platforms.Where(p => p.Name.Trim().ToLower() == name.Trim().ToLower()).FirstOrDefault()!;
        }

        public IList<Platform> GetPlatforms()
        {
            return dataContext.Platforms.OrderBy(p => p.Id).ToList();
        }

        public bool PlatformExist(int platformId)
        {
            return dataContext.Platforms.Any(p => p.Id == platformId);
        }

        //--------------------------------------------

        public bool CreatePlatform(Platform platform)
        {
            dataContext.Add(platform);
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
