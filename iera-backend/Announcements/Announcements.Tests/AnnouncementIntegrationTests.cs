using Announcements.Controllers;
using Announcements.Data;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Announcements.Tests
{
    public class AnnouncementIntegrationTests
    {
        public Mock<IAnnouncementRepository> _mockRepo;
        public AnnouncementController _controller;

        public AnnouncementIntegrationTests()
        {
            _mockRepo = new Mock<IAnnouncementRepository>();
            _controller = new AnnouncementController(_mockRepo.Object);
        }

        [Fact]
        public async Task GetAllAnnouncements_ReturnsOkResult_WithListOfAnnouncements()
        {
            // Arrange
            var announcements = new List<Announcement> { new Announcement(), new Announcement() };
            _mockRepo.Setup(repo => repo.GetAllAnnouncements()).ReturnsAsync(announcements);

            // Act
            var result = await _controller.GetAllAnnouncements();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<Announcement>>(okResult.Value);
            Assert.Equal(announcements.Count, returnValue.Count);
        }

        [Fact]
        public async Task GetAnnouncement_ReturnsOkResult_WithAnnouncement()
        {
            // Arrange
            var announcement = new Announcement { Id = "1" };
            _mockRepo.Setup(repo => repo.GetAnnouncement("1")).ReturnsAsync(announcement);

            // Act
            var result = await _controller.GetAnnouncement("1");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<Announcement>(okResult.Value);
            Assert.Equal(announcement.Id, returnValue.Id);
        }

        [Fact]
        public async Task GetAnnouncement_ReturnsNotFoundResult_WhenAnnouncementNotFound()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetAnnouncement("1")).ReturnsAsync((Announcement)null);

            // Act
            var result = await _controller.GetAnnouncement("1");

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        //[Fact]
        //public async Task AddAnnouncement_ReturnsCreatedAtActionResult()
        //{
        //    // Arrange
        //    var announcement = new Announcement { Id = "1" };

        //    var mockRepo = new Mock<IAnnouncementRepository>();

        //    // Mock the AddAnnouncement method to return a task that completes with a dummy object
        //    mockRepo.Setup(repo => repo.AddAnnouncement(It.IsAny<Announcement>())).ReturnsAsync(new object());

        //    var _controller = new AnnouncementController(mockRepo.Object);

        //    // Act
        //    var result = await _controller.AddAnnouncement(announcement);

        //    // Assert
        //    var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        //    var returnValue = Assert.IsType<Announcement>(createdAtActionResult.Value);
        //    Assert.Equal(announcement.Id, returnValue.Id);
        //    Assert.Equal("1", createdAtActionResult.RouteValues["id"]);
        //}



        [Fact]
        public async Task AddAnnouncement_ReturnsBadRequest_WhenAnnouncementIsNull()
        {
            // Act
            var result = await _controller.AddAnnouncement(null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("User is null", badRequestResult.Value);
        }

        [Fact]
        public async Task UpdateAnnouncement_ReturnsAcceptedResult()
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
        public async Task UpdateAnnouncement_ReturnsNotFoundResult_WhenAnnouncementNotFound()
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
        public async Task DeleteAnnouncement_ReturnsAcceptedResult()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.DeleteAnnouncement("1")).ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteAnnouncement("1");

            // Assert
            Assert.IsType<AcceptedResult>(result);
        }

        [Fact]
        public async Task DeleteAnnouncement_ReturnsNotFoundResult_WhenAnnouncementNotFound()
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