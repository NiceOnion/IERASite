// MongoDBContext.cs
using MongoDB.Driver;
using Users.Models;

public class MongoDBContext
{
    private readonly IMongoDatabase _database;

    public MongoDBContext(IConfiguration configuration)
    {
        var connectionString = configuration.GetSection("MongoDBSettings:ConnectionString").Value;
        var databaseName = configuration.GetSection("MongoDBSettings:DatabaseName").Value;
        var client = new MongoClient(connectionString);
        _database = client.GetDatabase(databaseName);
    }

    public IMongoCollection<User> Users => _database.GetCollection<User>("Users");
}
