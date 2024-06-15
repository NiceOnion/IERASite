using MongoDB.Driver;
using Users.Models;

public class MongoDBContext : IMongoDBContext
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



public class MongoDBSettings
{
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
}
