using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Moq;
using Users.Controllers;
using Users.Models;
using Xunit;

namespace Users.Tests
{
    public class UsersControllerIntegrationTests
    {
        private UsersController _controller;
        private Mock<IMongoCollection<User>> _mockCollection;

        public UsersControllerIntegrationTests()
        {
            // Mocking the collection
            _mockCollection = new Mock<IMongoCollection<User>>();

            // Instantiating the controller with the mock collection
            _controller = new UsersController(new MongoDBContext { Users = _mockCollection.Object });
        }

        [Fact]
        public async Task GetUsers_ReturnsSuccessStatusCode()
        {
            // Arrange
            var users = new List<User>();
            _mockCollection.Setup(c => c.Find(It.IsAny<System.Linq.Expressions.Expression<System.Func<User, bool>>>()).ToListAsync()).ReturnsAsync(users);

            // Act
            var result = await _controller.GetUsers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var model = Assert.IsAssignableFrom<IEnumerable<User>>(okResult.Value);
            Assert.Empty(model);
        }

        // Similarly, write tests for other controller actions
    }
}
