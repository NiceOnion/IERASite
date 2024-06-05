using RestSharp;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Announcements.IntegrationTests
{
    public class AnnouncementIntegrationTests
    {
        private readonly RestClient _client;

        public AnnouncementIntegrationTests()
        {
            _client = new RestClient("http://192.168.49.2:30998/api/Announcement");
        }

        [Fact]
        public async Task GetAllAnnouncements_ReturnsOkResult_WithAListOfAnnouncements()
        {
            // Act
            var request = new RestRequest("All", Method.GET);
            var response = await _client.ExecuteAsync<List<Announcement>>(request);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(response.Data);
        }

        [Fact]
        public async Task GetAnnouncement_ReturnsOkResult_WithAnAnnouncement()
        {
            // Arrange
            var announcementId = "1";

            // Act
            var request = new RestRequest($"{announcementId}", Method.GET);
            var response = await _client.ExecuteAsync<Announcement>(request);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(response.Data);
        }

        [Fact]
        public async Task AddAnnouncement_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var newAnnouncement = new Announcement { /* set properties */ };

            // Act
            var request = new RestRequest(Method.POST);
            request.AddJsonBody(newAnnouncement);
            var response = await _client.ExecuteAsync<Announcement>(request);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);
            Assert.NotNull(response.Data);
        }

        [Fact]
        public async Task UpdateAnnouncement_ReturnsAcceptedResult()
        {
            // Arrange
            var updatedAnnouncement = new Announcement { /* set properties */ };

            // Act
            var request = new RestRequest(Method.PUT);
            request.AddJsonBody(updatedAnnouncement);
            var response = await _client.ExecuteAsync(request);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.Accepted, response.StatusCode);
        }

        [Fact]
        public async Task DeleteAnnouncement_ReturnsAcceptedResult()
        {
            // Arrange
            var announcementId = "1";

            // Act
            var request = new RestRequest($"{announcementId}", Method.DELETE);
            var response = await _client.ExecuteAsync(request);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.Accepted, response.StatusCode);
        }
    }
}
