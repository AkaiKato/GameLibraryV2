using GameLibraryV2.Models;

namespace GameLibraryV2.Interfaces
{
    public interface IPublisherRepository
    {
        public Publisher GetPublisherById(int publisherId);

        public Publisher GetPublisherByName(string publisherName);

        public IList<Publisher> GetPublishers();

        public bool PublisherExists(int publisherId);

        public bool CreatePublisher(Publisher publisher);
    }
}
