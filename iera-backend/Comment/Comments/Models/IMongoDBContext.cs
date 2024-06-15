using MongoDB.Driver;

namespace Comments.Models
{
    public interface IMongoDBContext
    {
        IMongoCollection<Comment> Comments { get; }
    }
}
