using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Mongo2Go;
using MongoDB.Driver;
using Users.Models;
using Xunit;

public class UsersIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly MongoDbRunner _mongoRunner;
    private readonly IMongoDatabase _database;

    public UsersIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _mongoRunner = MongoDbRunner.Start();
        var client = new MongoClient(_mongoRunner.ConnectionString);
        _database = client.GetDatabase("test");

        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Replace the MongoDBContext with a test context using in-memory MongoDB
                services.AddSingleton<IMongoDatabase>(_database);
                services.AddSingleton<MongoDBContext>();
            });

            builder.ConfigureAppConfiguration((context, config) =>
            {
                var inMemorySettings = new Dictionary<string, string> {
                    {"MongoDBSettings:ConnectionString", _mongoRunner.ConnectionString},
                    {"MongoDBSettings:DatabaseName", "test"}
                };

                config.AddInMemoryCollection(inMemorySettings);
            });
        });
    }

    [Fact]
    public async Task GetUsers_ReturnsEmptyList()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/users");

        // Assert
        response.EnsureSuccessStatusCode();
        var users = await response.Content.ReadFromJsonAsync<List<User>>();
        Assert.Empty(users);
    }

    [Fact]
    public async Task CreateUser_AddsUserToDatabase()
    {
        // Arrange
        var client = _factory.CreateClient();
        var newUser = new User { Name = "Test User", Email = "test@example.com" };

        // Act
        var response = await client.PostAsJsonAsync("/api/users", newUser);

        // Assert
        response.EnsureSuccessStatusCode();
        var createdUser = await response.Content.ReadFromJsonAsync<User>();
        Assert.NotNull(createdUser.Id);
        Assert.Equal(newUser.Name, createdUser.Name);
        Assert.Equal(newUser.Email, createdUser.Email);
    }

    [Fact]
    public async Task UpdateUser_UpdatesExistingUser()
    {
        // Arrange
        var client = _factory.CreateClient();
        var newUser = new User { Name = "Test User", Email = "test@example.com" };
        var postResponse = await client.PostAsJsonAsync("/api/users", newUser);
        var createdUser = await postResponse.Content.ReadFromJsonAsync<User>();

        createdUser.Name = "Updated User";

        // Act
        var putResponse = await client.PutAsJsonAsync($"/api/users/{createdUser.Id}", createdUser);

        // Assert
        putResponse.EnsureSuccessStatusCode();

        var getResponse = await client.GetAsync($"/api/users");
        var users = await getResponse.Content.ReadFromJsonAsync<List<User>>();
        Assert.Single(users);
        Assert.Equal("Updated User", users[0].Name);
    }

    [Fact]
    public async Task DeleteUser_RemovesUserFromDatabase()
    {
        // Arrange
        var client = _factory.CreateClient();
        var newUser = new User { Name = "Test User", Email = "test@example.com" };
        var postResponse = await client.PostAsJsonAsync("/api/users", newUser);
        var createdUser = await postResponse.Content.ReadFromJsonAsync<User>();

        // Act
        var deleteResponse = await client.DeleteAsync($"/api/users/{createdUser.Id}");

        // Assert
        deleteResponse.EnsureSuccessStatusCode();

        var getResponse = await client.GetAsync($"/api/users");
        var users = await getResponse.Content.ReadFromJsonAsync<List<User>>();
        Assert.Empty(users);
    }
}
