using GameLibraryV2.Data;
using GameLibraryV2.Interfaces;
using GameLibraryV2.Models;

namespace GameLibraryV2.Repositories
{
    public class DeveloperRepository : IDeveloperRepository
    {
        private readonly DataContext dataContext;

        public DeveloperRepository(DataContext context)
        {
            dataContext = context;
        }

        public bool DeveloperExists(int developerId)
        {
            return dataContext.Developers.Any(d => d.Id == developerId);
        }

        public Developer GetDeveloperById(int developerId)
        {
            return dataContext.Developers.FirstOrDefault(d => d.Id == developerId)!;    
        }

        public Developer GetDeveloperByName(string developerName)
        {
            return dataContext.Developers.FirstOrDefault(d => d.Name.Trim().ToLower() == developerName.Trim().ToLower())!;
        }

        public IList<Developer> GetDevelopers()
        {
            return dataContext.Developers.OrderBy(d => d.Id).ToList();
        }

        //------------------------------------------------------------------

        public bool CreateDeveloper(Developer developer)
        {
            dataContext.Add(developer);
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
