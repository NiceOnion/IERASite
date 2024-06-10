using Comments.Models;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Newtonsoft.Json;
using System.Text;

public class CommentsIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public CommentsIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task AddComment_ReturnsCreatedResponse()
    {
        // Arrange
        var newComment = new Comment { PostId = "post1", UserID = "user1", Body = "Integration test comment", PostTime = Timestamp.FromDateTime(DateTime.UtcNow) };
        var content = new StringContent(JsonConvert.SerializeObject(newComment), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/Comment", content);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task GetCommentsByPost_ReturnsOkResponse()
    {
        // Act
        var response = await _client.GetAsync("/api/Comment/Post/0");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task DeleteComment_ReturnsNoContentResponse()
    {
        // Act
        var response = await _client.DeleteAsync("/api/Comment/1");

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task UpdateComment_ReturnsNoContentResponse()
    {
        // Arrange
        var updatedComment = new Comment { Id = "1", PostId = "post1", UserID = "user1", Body = "Updated integration test comment", PostTime = Timestamp.FromDateTime(DateTime.UtcNow) };
        var content = new StringContent(JsonConvert.SerializeObject(updatedComment), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PutAsync("/api/Comment/1", content);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(System.Net.HttpStatusCode.NoContent, response.StatusCode);
    }
}
