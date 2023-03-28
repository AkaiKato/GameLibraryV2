using GameLibraryV2.Data;
using GameLibraryV2.Interfaces;
using GameLibraryV2.Models;
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

        public DLC GetDLCConnById(int parentGameId, int dlcGame)
        {
            return dataContext.DLCs.Include(p => p.ParentGame).Include(d => d.DLCGame).Where(d => d.ParentGame.Id == parentGameId && d.DLCGame.Id == dlcGame).FirstOrDefault()!;
        }

        public bool DLCExists(int parentGameId, int dlcGameId)
        {
            return dataContext.DLCs.Any(d => d.ParentGame.Id == parentGameId && d.DLCGame.Id == dlcGameId);
        }

        public bool DLCExistsByConnId(int dlc)
        {
            return dataContext.DLCs.Any(d => d.Id == dlc);
        }

        public bool DLCCreate(DLC dlc)
        {
            dataContext.Add(dlc);
            return Save();
        }

        public bool DLCDelete(DLC dlc)
        {
            dataContext.Remove(dlc);
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
