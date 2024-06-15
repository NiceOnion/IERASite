using Announcements.Data;
using MongoDB.Driver;

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

    public IMongoCollection<Announcement> Announcements => _database.GetCollection<Announcement>("Announcements");
}



public class MongoDBSettings
{
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
}
