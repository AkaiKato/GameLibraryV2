using GameLibraryV2.Models.Common;

namespace GameLibraryV2.Interfaces
{
    public interface IDLCRepository
    {
        public Task<bool> DLCExistsAsync(int parentGameId, int dlcGame);

        public Task<bool> DLCExistsByConnIdAsync(int dlc);

        public Task<DLC> GetDLCConnByIdAsync(int dlcConnId);

        public void DLCCreate(DLC dlc);

        public void DLCDelete(DLC dlc);

        public Task SaveDLCAsync();
    }
}
