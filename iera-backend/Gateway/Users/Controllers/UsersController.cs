using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Driver;
using Users.Models;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IMongoCollection<User> _usersCollection;
    private RabbitMQPublisher rabbitMQPublisher;
    private readonly HttpClient _httpClient;

    public UsersController(IMongoDBContext dbContext)
    {
        _usersCollection = dbContext.Users;
        rabbitMQPublisher = new RabbitMQPublisher();
        _httpClient = new HttpClient();
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        var users = await _usersCollection.Find(user => true).ToListAsync();
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUserById(string id)
    {
        var user = await _usersCollection.Find(u => u.Id == id).FirstOrDefaultAsync();
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }

    [HttpPost]
    public async Task CreateUserAsync(User user)
    {
        try
        {
            string jsonString = JsonSerializer.Serialize(user);
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(
                "https://us-central1-ierawebsite-426320.cloudfunctions.net/UserSignUp",
                content);
            Console.WriteLine("Sent request");
            Console.WriteLine(response.Content);

            response.EnsureSuccessStatusCode(); // Throw if not successful

            string responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseBody); // Handle response as needed
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"Error creating user: {e.Message}");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(string id, User user)
    {
        user.Id = id;
        var result = await _usersCollection.ReplaceOneAsync(u => u.Id == id, user);
        if (result.ModifiedCount == 0)
            return NotFound();
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var result = await _usersCollection.DeleteOneAsync(user => user.Id == id);
        if (result.DeletedCount == 0)
            return NotFound();

        // Publish user delete event to RabbitMQ
        
        rabbitMQPublisher.SendMessage(id);
        rabbitMQPublisher.Close();

        return Ok();
    }
}
