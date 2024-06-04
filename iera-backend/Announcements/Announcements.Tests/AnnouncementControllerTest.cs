using Announcements.Controllers;
using Announcements.Data;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Announcements.Tests
{
    [TestClass]
    public class AnnouncementControllerTests
    {
        [Fact]
        public async Task GetAllAnnouncements_Returns_OkResult_With_ListOfAnnouncements()
        {
            // Arrange
            var fakeAnnouncements = new List<Announcement>
            {
                new Announcement { Id = "1", Title = "Announcement 1", Content = "Content 1" },
                new Announcement { Id = "2", Title = "Announcement 2", Content = "Content 2" },
                // Add more fake announcements as needed
            };

            var announcementRepositoryMock = new Mock<IAnnouncementRepository>();
            announcementRepositoryMock.Setup(repo => repo.GetAllAnnouncements())
                .ReturnsAsync(fakeAnnouncements);

            var controller = new AnnouncementController(announcementRepositoryMock.Object);

            // Act
            var result = await controller.GetAllAnnouncements();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var announcements = Assert.IsType<List<Announcement>>(okResult.Value);
            Assert.Equal(fakeAnnouncements, announcements);
        }

        // Add more unit tests for other controller actions as needed
    }
}
