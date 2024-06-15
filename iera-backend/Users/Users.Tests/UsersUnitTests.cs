using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MongoDB.Bson;
using MongoDB.Driver;
using Users.Models;
using Xunit;

public class UsersControllerTests
{
    private readonly Mock<IMongoCollection<User>> _mockCollection;
    private readonly Mock<IAsyncCursor<User>> _mockCursor;
    private readonly UsersController _controller;

    public UsersControllerTests()
    {
        _mockCollection = new Mock<IMongoCollection<User>>();
        _mockCursor = new Mock<IAsyncCursor<User>>();

        var mockDbContext = new Mock<IMongoDBContext>();
        mockDbContext.Setup(db => db.Users).Returns(_mockCollection.Object);

        _controller = new UsersController(mockDbContext.Object);
    }

    [Fact]
    public async Task GetUsers_ReturnsEmptyList()
    {
        // Arrange
        var users = new List<User>();
        _mockCursor.SetupSequence(c => c.MoveNext(It.IsAny<CancellationToken>())).Returns(true).Returns(false);
        _mockCursor.Setup(c => c.Current).Returns(users);
        _mockCollection.Setup(c => c.FindAsync(It.IsAny<FilterDefinition<User>>(), It.IsAny<FindOptions<User>>(), It.IsAny<CancellationToken>())).ReturnsAsync(_mockCursor.Object);

        // Act
        var result = await _controller.GetUsers();

        // Assert
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        var returnUsers = okResult.Value as List<User>;
        returnUsers.Should().BeEmpty();
    }

    [Fact]
    public async Task UpdateUser_UpdatesExistingUser()
    {
        // Arrange
        var userId = ObjectId.GenerateNewId().ToString();
        var user = new User { Id = userId, Name = "Updated User", Email = "updated@example.com" };
        var updateResult = new ReplaceOneResult.Acknowledged(1, 1, userId);

        _mockCollection
            .Setup(c => c.ReplaceOneAsync(
                It.IsAny<FilterDefinition<User>>(),
                user,
                It.IsAny<ReplaceOptions>(),
                default))
            .ReturnsAsync(updateResult);

        // Act
        var result = await _controller.UpdateUser(userId, user);

        // Assert
        result.Should().BeOfType<OkResult>();
    }

    [Fact]
    public async Task DeleteUser_RemovesUserFromDatabase()
    {
        // Arrange
        var userId = ObjectId.GenerateNewId().ToString();
        var deleteResult = new DeleteResult.Acknowledged(1);

        _mockCollection.Setup(c => c.DeleteOneAsync(It.IsAny<FilterDefinition<User>>(), default)).ReturnsAsync(deleteResult);

        // Act
        var result = await _controller.DeleteUser(userId);

        // Assert
        result.Should().BeOfType<OkResult>();
    }

    [Fact]
    public async Task DeleteUser_ReturnsNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = ObjectId.GenerateNewId().ToString();
        var deleteResult = new DeleteResult.Acknowledged(0);

        _mockCollection.Setup(c => c.DeleteOneAsync(It.IsAny<FilterDefinition<User>>(), default)).ReturnsAsync(deleteResult);

        // Act
        var result = await _controller.DeleteUser(userId);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }
}
