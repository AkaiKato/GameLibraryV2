using GameLibraryV2.Models.Common;

namespace GameLibraryV2.Interfaces
{
    public interface ISystemRequirements
    {
        public Task<bool> SystemRequirementsExists(int id);

        public void UpdateSystemRequirements(SystemRequirements systemRequirements);

        public void DeleteSystemRequirements(SystemRequirements systemRequirements);

        public Task SaveSystemRequirementsAsync();
    }
}
