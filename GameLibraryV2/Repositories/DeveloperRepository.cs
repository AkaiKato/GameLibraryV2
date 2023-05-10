using GameLibraryV2.Data;
using GameLibraryV2.Interfaces;
using GameLibraryV2.Models.Common;
using Microsoft.EntityFrameworkCore;

namespace GameLibraryV2.Repositories
{
    public class DeveloperRepository : IDeveloperRepository
    {
        private readonly DataContext dataContext;

        public DeveloperRepository(DataContext context)
        {
            dataContext = context;
        }

        public async Task<bool> DeveloperExistsAsync(int developerId)
        {
            return await dataContext.Developers.AnyAsync(d => d.Id == developerId);
        }

        public async Task<bool> DeveloperNameAlreadyExistsAsync(int developerId, string developerName)
        {
            return await dataContext.Developers
                .AnyAsync(d => d.Name.Trim().ToLower() == developerName.Trim().ToLower() && d.Id != developerId);
        }

        public async Task<IList<Developer>> GetDevelopersThatContainsStringAsync(string searchString)
        {
            return await dataContext.Developers
                .Where(d => d.Name.ToLower().Contains(searchString.ToLower().Trim().ToString()))
                .ToListAsync();
        }

        public async Task<Developer> GetDeveloperByIdAsync(int developerId)
        {
            return await dataContext.Developers.FirstOrDefaultAsync(d => d.Id == developerId)!;    
        }

        public async Task<Developer> GetDeveloperByNameAsync(string developerName)
        {
            return await dataContext.Developers.FirstOrDefaultAsync(d => d.Name.Trim().ToLower() == developerName.Trim().ToLower())!;
        }

        public async Task<IList<Developer>> GetDevelopersAsync()
        {
            return await dataContext.Developers.OrderBy(d => d.Id).ToListAsync();
        }

        //------------------------------------------------------------------

        public void CreateDeveloper(Developer developer)
        {
            dataContext.Add(developer);
        }

        public void UpdateDeveloper(Developer developer)
        {
            dataContext.Update(developer);
        }

        public void DeleteDeveloper(Developer developer)
        {
            dataContext.Remove(developer);
        }

        public async Task SaveDeveloperAsync()
        {
            await dataContext.SaveChangesAsync();
        }
    }
}
