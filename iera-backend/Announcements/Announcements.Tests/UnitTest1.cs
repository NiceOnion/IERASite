using Announcements.Controllers;
using Announcements.Data;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc;

namespace Announcements.Tests
{
    public class AnnouncementControllerTests
    {
        private readonly Mock<IAnnouncementRepository> _mockRepo;
        private readonly AnnouncementController _controller;

        public AnnouncementControllerTests()
        {
            _mockRepo = new Mock<IAnnouncementRepository>();
            _controller = new AnnouncementController(_mockRepo.Object);
        }

        [Fact]
        public async Task GetAllAnnouncements_ReturnsOkResult_WithAListOfAnnouncements()
        {
            // Arrange
            var announcements = new List<Announcement> { new Announcement(), new Announcement() };
            _mockRepo.Setup(repo => repo.GetAllAnnouncements()).ReturnsAsync(announcements);

            // Act
            var result = await _controller.GetAllAnnouncements();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<Announcement>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task GetAnnouncement_ReturnsOkResult_WithAnAnnouncement()
        {
            // Arrange
            var announcement = new Announcement { Id = "1" };
            _mockRepo.Setup(repo => repo.GetAnnouncement("1")).ReturnsAsync(announcement);

            // Act
            var result = await _controller.GetAnnouncement("1");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<Announcement>(okResult.Value);
            Assert.Equal("1", returnValue.Id);
        }

        [Fact]
        public async Task GetAnnouncement_ReturnsNotFound_WhenAnnouncementNotFound()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetAnnouncement("1")).ReturnsAsync((Announcement)null);

            // Act
            var result = await _controller.GetAnnouncement("1");

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task AddAnnouncement_ReturnsCreatedAtActionResult_WithTheAnnouncement()
        {
            // Arrange
            var announcement = new Announcement { Id = "1" };
            _mockRepo.Setup(repo => repo.AddAnnouncement(announcement)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.AddAnnouncement(announcement);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnValue = Assert.IsType<Announcement>(createdAtActionResult.Value);
            Assert.Equal("1", createdAtActionResult.RouteValues["id"]);
            Assert.Equal(announcement, returnValue);
        }

        [Fact]
        public async Task AddAnnouncement_ReturnsBadRequest_WhenAnnouncementIsNull()
        {
            // Act
            var result = await _controller.AddAnnouncement(null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Announcement is null", badRequestResult.Value);
        }

        [Fact]
        public async Task UpdateAnnouncement_ReturnsAccepted_WhenUpdateSuccessful()
        {
            // Arrange
            var announcement = new Announcement { Id = "1" };
            _mockRepo.Setup(repo => repo.UpdateAnnouncement(announcement)).ReturnsAsync(true);

            // Act
            var result = await _controller.UpdateAnnouncement(announcement);

            // Assert
            Assert.IsType<AcceptedResult>(result);
        }

        [Fact]
        public async Task UpdateAnnouncement_ReturnsNotFound_WhenUpdateFails()
        {
            // Arrange
            var announcement = new Announcement { Id = "1" };
            _mockRepo.Setup(repo => repo.UpdateAnnouncement(announcement)).ReturnsAsync(false);

            // Act
            var result = await _controller.UpdateAnnouncement(announcement);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task UpdateAnnouncement_ReturnsBadRequest_WhenAnnouncementIsNull()
        {
            // Act
            var result = await _controller.UpdateAnnouncement(null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Announcement is null", badRequestResult.Value);
        }

        [Fact]
        public async Task DeleteAnnouncement_ReturnsAccepted_WhenDeleteSuccessful()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.DeleteAnnouncement("1")).ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteAnnouncement("1");

            // Assert
            Assert.IsType<AcceptedResult>(result);
        }

        [Fact]
        public async Task DeleteAnnouncement_ReturnsNotFound_WhenDeleteFails()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.DeleteAnnouncement("1")).ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteAnnouncement("1");

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
