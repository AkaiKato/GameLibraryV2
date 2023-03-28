using GameLibraryV2.Models;

namespace GameLibraryV2.Interfaces
{
    public interface IDLCRepository
    {
        public bool DLCExists(int parentGameId, int dlcGame);

        public bool DLCExistsByConnId(int dlc);

        public DLC GetDLCConnById(int parentGameId, int dlcGame);

        public bool DLCCreate(DLC dlc);

        public bool DLCDelete(DLC dlc);
    }
}
