using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using Users.Models;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IMongoCollection<User> _usersCollection;

    public UsersController(IMongoDBContext dbContext)
    {
        _usersCollection = dbContext.Users;
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
    public async Task<ActionResult<User>> CreateUser(User user)
    {
        user.Id = ObjectId.GenerateNewId().ToString();
        await _usersCollection.InsertOneAsync(user);
        return CreatedAtAction(nameof(GetUsers), new { id = user.Id }, user);
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
        return Ok();
    }
}
