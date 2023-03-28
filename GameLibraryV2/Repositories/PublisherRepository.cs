using GameLibraryV2.Data;
using GameLibraryV2.Interfaces;
using GameLibraryV2.Models;

namespace GameLibraryV2.Repositories
{
    public class PublisherRepository : IPublisherRepository
    {
        private readonly DataContext dataContext;

        public PublisherRepository(DataContext context)
        {
            dataContext = context;
        }

        public Publisher GetPublisherById(int publisherId)
        {
            return dataContext.Publishers.Where(p => p.Id == publisherId).FirstOrDefault()!;
        }

        public Publisher GetPublisherByName(string publisherName)
        {
            return dataContext.Publishers.Where(p => p.Name.Trim().ToLower() == publisherName.Trim().ToLower()).FirstOrDefault()!;
        }
        
        public IList<Publisher> GetPublishers()
        {
            return dataContext.Publishers.OrderBy(p => p.Id).ToList();
        }

        public bool PublisherExists(int publisherId)
        {
            return dataContext.Publishers.Any(p => p.Id == publisherId);
        }

        public bool PublisherNameAlreadyExists(int publisherId, string publisherName)
        {
            return dataContext.Publishers.Any(d => d.Name.Trim().ToLower() == publisherName.Trim().ToLower() && d.Id != publisherId);
        }

        //--------------------------------------------

        public bool CreatePublisher(Publisher publisher)
        {
            dataContext.Add(publisher);
            return Save();
        }

        public bool UpdatePublisher(Publisher publisher)
        {
            dataContext.Update(publisher);
            return Save();
        }

        public bool DeletePublisher(Publisher publisher) 
        {
            dataContext.Remove(publisher);
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
