using GameLibraryV2.Models.Common;

namespace GameLibraryV2.Interfaces
{
    public interface IDeveloperRepository
    {
        public Task<Developer> GetDeveloperByIdAsync(int developerId);

        public Task<Developer> GetDeveloperByNameAsync(string developerName);

        public Task<IList<Developer>> GetDevelopersThatContainsStringAsync(string searchString);

        public Task<IList<Developer>> GetDevelopersAsync();
        
        public Task<bool> DeveloperExistsAsync(int developerId);

        public Task<bool> DeveloperNameAlreadyExistsAsync(int developerId, string developerName);

        public void CreateDeveloper(Developer developer);

        public void UpdateDeveloper(Developer developer);

        public void DeleteDeveloper(Developer developer);

        public Task SaveDeveloperAsync();
    }
}
