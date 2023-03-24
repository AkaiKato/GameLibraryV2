using GameLibraryV2.Models;

namespace GameLibraryV2.Interfaces
{
    public interface IDLCRepository
    {
        public bool DLCExists(int dlcId);

        public bool DLCDelete(DLC dlc);
    }
}
