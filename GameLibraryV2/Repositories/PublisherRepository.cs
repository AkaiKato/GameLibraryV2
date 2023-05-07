using GameLibraryV2.Data;
using GameLibraryV2.Interfaces;
using GameLibraryV2.Models.Common;
using Microsoft.EntityFrameworkCore;

namespace GameLibraryV2.Repositories
{
    public class PublisherRepository : IPublisherRepository
    {
        private readonly DataContext dataContext;

        public PublisherRepository(DataContext context)
        {
            dataContext = context;
        }

        public async Task<Publisher> GetPublisherByIdAsync(int publisherId)
        {
            return await dataContext.Publishers.FirstOrDefaultAsync(p => p.Id == publisherId)!;
        }

        public async Task<Publisher> GetPublisherByNameAsync(string publisherName)
        {
            return await dataContext.Publishers.FirstOrDefaultAsync(p => p.Name.Trim().ToLower() == publisherName.Trim().ToLower())!;
        }
        
        public async Task<IList<Publisher>> GetPublishersAsync()
        {
            return await dataContext.Publishers.OrderBy(p => p.Id).ToListAsync();
        }

        public async Task<bool> PublisherExistsAsync(int publisherId)
        {
            return await dataContext.Publishers.AnyAsync(p => p.Id == publisherId);
        }

        public async Task<bool> PublisherNameAlreadyExistsAsync(int publisherId, string publisherName)
        {
            return await dataContext.Publishers.AnyAsync(d => d.Name.Trim().ToLower() == publisherName.Trim().ToLower() && d.Id != publisherId);
        }

        //--------------------------------------------

        public void CreatePublisher(Publisher publisher)
        {
            dataContext.Add(publisher);
        }

        public void UpdatePublisher(Publisher publisher)
        {
            dataContext.Update(publisher);
        }

        public void DeletePublisher(Publisher publisher) 
        {
            dataContext.Remove(publisher);
        }

        public async Task SavePublisherAsync()
        {
            await dataContext.SaveChangesAsync();
        }
    }
}
