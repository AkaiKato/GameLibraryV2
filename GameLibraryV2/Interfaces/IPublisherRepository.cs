using GameLibraryV2.Models.Common;

namespace GameLibraryV2.Interfaces
{
    public interface IPublisherRepository
    {
        public Task<Publisher> GetPublisherByIdAsync(int publisherId);

        public Task<Publisher> GetPublisherByNameAsync(string publisherName);

        public Task<IList<Publisher>> GetDevelopersThatContainsStringAsync(string searchString);

        public Task<IList<Publisher>> GetPublishersAsync();

        public Task<bool> PublisherExistsAsync(int publisherId);

        public Task<bool> PublisherNameAlreadyExistsAsync(int publisherId, string publisherName);

        public void CreatePublisher(Publisher publisher);

        public void UpdatePublisher(Publisher publisher);

        public void DeletePublisher(Publisher publisher);

        public Task SavePublisherAsync();
    }
}
