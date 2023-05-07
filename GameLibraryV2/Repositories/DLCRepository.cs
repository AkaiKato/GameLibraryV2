using GameLibraryV2.Data;
using GameLibraryV2.Interfaces;
using GameLibraryV2.Models.Common;
using Microsoft.EntityFrameworkCore;

namespace GameLibraryV2.Repositories
{
    public class DLCRepository : IDLCRepository
    {
        private readonly DataContext dataContext;

        public DLCRepository(DataContext context)
        {
            dataContext = context;
        }

        public async Task<DLC> GetDLCConnByIdAsync(int parentGameId, int dlcGame)
        {
            return await dataContext.DLCs
                .Include(p => p.ParentGame)
                .Include(d => d.DLCGame)
                .FirstOrDefaultAsync(d => d.ParentGame.Id == parentGameId && d.DLCGame.Id == dlcGame);
        }

        public async Task<bool> DLCExistsAsync(int parentGameId, int dlcGameId)
        {
            return await dataContext.DLCs.AnyAsync(d => d.ParentGame.Id == parentGameId && d.DLCGame.Id == dlcGameId);
        }

        public async Task<bool> DLCExistsByConnIdAsync(int dlc)
        {
            return await dataContext.DLCs.AnyAsync(d => d.Id == dlc);
        }

        public void DLCCreate(DLC dlc)
        {
            dataContext.Add(dlc);
        }

        public void DLCDelete(DLC dlc)
        {
            dataContext.Remove(dlc);
        }

        public async Task SaveDLCAsync()
        {
            await dataContext.SaveChangesAsync();
        }
    }
}
