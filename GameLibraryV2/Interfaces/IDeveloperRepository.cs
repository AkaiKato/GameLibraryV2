using GameLibraryV2.Models;

namespace GameLibraryV2.Interfaces
{
    public interface IDeveloperRepository
    {
        public Developer GetDeveloperById(int developerId);

        public Developer GetDeveloperByName(string developerName);

        public IList<Developer> GetDevelopers();
        
        public bool DeveloperExists(int developerId);

        public bool CreateDeveloper(Developer developer);
    }
}
