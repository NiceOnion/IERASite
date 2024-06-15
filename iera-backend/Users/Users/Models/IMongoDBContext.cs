// IMongoDBContext.cs
using MongoDB.Driver;
using Users.Models;

public interface IMongoDBContext
{
    IMongoCollection<User> Users { get; }
}
