using GameLibraryV2.Data;
using GameLibraryV2.Interfaces;
using GameLibraryV2.Models.Common;
using Microsoft.EntityFrameworkCore;

namespace GameLibraryV2.Repositories
{
    public class SystemRequirementsRepository : ISystemRequirements
    {

        private readonly DataContext dataContext;

        public SystemRequirementsRepository(DataContext context)
        {
            dataContext = context;
        }

        public async Task<SystemRequirements> GetSystemRequirementsAsync(int id)
        {
            return await dataContext.SystemRequirements.FindAsync(id);
        }

        public async Task<bool> SystemRequirementsExists(int id)
        {
            return await dataContext.SystemRequirements.AnyAsync(x => x.Id == id);
        }

        public void UpdateSystemRequirements(SystemRequirements systemRequirements)
        {
            dataContext.SystemRequirements.Update(systemRequirements);
        }

        public void DeleteSystemRequirements(SystemRequirements systemRequirements)
        {
            dataContext.SystemRequirements.Remove(systemRequirements);
        }

        public async Task SaveSystemRequirementsAsync()
        {
            await dataContext.SaveChangesAsync();
        }
    }
}
