using GameLibraryV2.Models.Common;

namespace GameLibraryV2.Interfaces
{
    public interface IPublisherRepository
    {
        public Publisher GetPublisherById(int publisherId);

        public Publisher GetPublisherByName(string publisherName);

        public IList<Publisher> GetPublishers();

        public bool PublisherExists(int publisherId);

        public bool PublisherNameAlreadyExists(int publisherId, string publisherName);

        public bool CreatePublisher(Publisher publisher);

        public bool UpdatePublisher(Publisher publisher);

        public bool DeletePublisher(Publisher publisher);
    }
}
