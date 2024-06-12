// using Announcements.Controllers;
// using Announcements.Data;
// using Google.Cloud.Firestore;
// using Microsoft.AspNetCore.Mvc;
// using System.Collections.Generic;
// using System.Threading.Tasks;
// using Xunit;

// namespace Announcements.Tests
// {
//     public class AnnouncementIntegrationTests
//     {
//         private readonly AnnouncementRepository _repository;
//         private readonly AnnouncementController _controller;

//         public AnnouncementIntegrationTests()
//         {
//             _repository = new AnnouncementRepository();
//             _controller = new AnnouncementController(_repository);
//         }

//         [Fact]
//         public async Task GetAllAnnouncements_ReturnsOkResult_WithListOfAnnouncements()
//         {
//             // Arrange
//             var announcement1 = new Announcement { Title = "Announcement 1" };
//             var announcement2 = new Announcement { Title = "Announcement 2" };
//             var docRef1 = await _repository.AddAnnouncement(announcement1);
//             var docRef2 = await _repository.AddAnnouncement(announcement2);

//             // Act
//             var result = await _controller.GetAllAnnouncements();

//             // Assert
//             var okResult = Assert.IsType<OkObjectResult>(result.Result);
//             var returnValue = Assert.IsType<List<Announcement>>(okResult.Value);

//             // Clean up
//             await _repository.DeleteAnnouncement(docRef1.Id);
//             await _repository.DeleteAnnouncement(docRef2.Id);
//         }

//         [Fact]
//         public async Task GetAnnouncement_ReturnsOkResult_WithAnnouncement()
//         {
//             // Arrange
//             var announcement = new Announcement { Title = "Announcement 1" };
//             var docRef = await _repository.AddAnnouncement(announcement);

//             // Act
//             var result = await _controller.GetAnnouncement(docRef.Id);

//             // Assert
//             var okResult = Assert.IsType<OkObjectResult>(result.Result);
//             var returnValue = Assert.IsType<Announcement>(okResult.Value);
//             Assert.Equal(announcement.Title, returnValue.Title);

//             // Clean up
//             await _repository.DeleteAnnouncement(docRef.Id);
//         }

//         [Fact]
//         public async Task GetAnnouncement_ReturnsNotFoundResult_WhenAnnouncementNotFound()
//         {
//             // Act
//             var result = await _controller.GetAnnouncement("non-existent-id");

//             // Assert
//             Assert.IsType<NotFoundResult>(result.Result);
//         }

//         [Fact]
//         public async Task AddAnnouncement_ReturnsCreatedAtActionResult()
//         {
//             // Arrange
//             var announcement = new Announcement { Title = "Announcement 1" };

//             // Act
//             var result = await _controller.AddAnnouncement(announcement);

//             // Assert
//             var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
//             var returnValue = Assert.IsType<Announcement>(createdAtActionResult.Value);
//             Assert.Equal(announcement.Title, returnValue.Title);

//             // Clean up
//             await _repository.DeleteAnnouncement(createdAtActionResult.RouteValues["id"].ToString());
//         }

//         [Fact]
//         public async Task AddAnnouncement_ReturnsBadRequest_WhenAnnouncementIsNull()
//         {
//             // Act
//             var result = await _controller.AddAnnouncement(null);

//             // Assert
//             var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
//             Assert.Equal("User is null", badRequestResult.Value);
//         }

//         [Fact]
//         public async Task UpdateAnnouncement_ReturnsAcceptedResult()
//         {
//             // Arrange
//             var announcement = new Announcement { Title = "Announcement 1" };
//             var docRef = await _repository.AddAnnouncement(announcement);

//             announcement.Id = docRef.Id;  // Set the ID to the Firestore-generated ID
//             announcement.Title = "Updated Title";

//             // Act
//             var result = await _controller.UpdateAnnouncement(announcement);

//             // Assert
//             Assert.IsType<AcceptedResult>(result);

//             // Clean up
//             await _repository.DeleteAnnouncement(docRef.Id);
//         }

//         [Fact]
//         public async Task UpdateAnnouncement_ReturnsNotFoundResult_WhenAnnouncementNotFound()
//         {
//             // Arrange
//             var announcement = new Announcement { Id = "non-existent-id", Title = "Announcement 1" };

//             // Act
//             var result = await _controller.UpdateAnnouncement(announcement);

//             // Assert
//             Assert.IsType<NotFoundResult>(result);
//         }

//         [Fact]
//         public async Task DeleteAnnouncement_ReturnsAcceptedResult()
//         {
//             // Arrange
//             var announcement = new Announcement { Title = "Announcement 1" };
//             var docRef = await _repository.AddAnnouncement(announcement);

//             // Act
//             var result = await _controller.DeleteAnnouncement(docRef.Id);

//             // Assert
//             Assert.IsType<AcceptedResult>(result);
//         }

//         [Fact]
//         public async Task DeleteAnnouncement_ReturnsNotFoundResult_WhenAnnouncementNotFound()
//         {
//             // Act
//             var result = await _controller.DeleteAnnouncement("non-existent-id");

//             // Assert
//             Assert.IsType<NotFoundResult>(result);
//         }
//     }
// }
