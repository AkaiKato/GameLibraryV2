using GameLibraryV2.Models;

namespace GameLibraryV2.Interfaces
{
    public interface IDLCRepository
    {
        public Game GetDLCById(int dlcId);

        public Game GetDLCByName(string dlcName);

        public IList<Game> GetDLCs();

        public bool DLCExists(int dlcId);

        public bool DLCDelete(DLC dlc);
    }
}
