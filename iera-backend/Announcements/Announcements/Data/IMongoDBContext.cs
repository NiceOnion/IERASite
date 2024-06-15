using MongoDB.Driver;

namespace Announcements.Data
{
    public interface IMongoDBContext
    {
        IMongoCollection<Announcement> Announcements { get; }
    }
}
