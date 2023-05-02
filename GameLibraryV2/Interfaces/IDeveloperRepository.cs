using GameLibraryV2.Models.Common;

namespace GameLibraryV2.Interfaces
{
    public interface IDeveloperRepository
    {
        public Developer GetDeveloperById(int developerId);

        public Developer GetDeveloperByName(string developerName);

        public IList<Developer> GetDevelopers();
        
        public bool DeveloperExists(int developerId);

        public bool DeveloperNameAlreadyExists(int developerId, string developerName);

        public bool CreateDeveloper(Developer developer);

        public bool UpdateDeveloper(Developer developer);

        public bool DeleteDeveloper(Developer developer);
    }
}
