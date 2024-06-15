//using System;
//using System.Net.Http;
//using System.Threading.Tasks;
//using Xunit;

//public class OcelotGatewayIntegrationTests : IDisposable
//{
//    private readonly HttpClient _httpClient;

//    public OcelotGatewayIntegrationTests()
//    {
//        // Initialize HttpClient with Ocelot gateway base URL
//        _httpClient = new HttpClient
//        {
//            BaseAddress = new Uri("https://localhost:32770")
//        };
//    }

//    public void Dispose()
//    {
//        _httpClient.Dispose();
//    }

//    [Theory]
//    [InlineData("/Event")]
//    [InlineData("/Announcement/All")]
//    [InlineData("/Announcement/1")]
//    [InlineData("/Announcement")]
//    [InlineData("/Users")]
//    public async Task GatewayRoutes_ShouldReturn200StatusCode(string route)
//    {
//        // Act
//        var response = await _httpClient.GetAsync(route);

//        // Assert
//        Assert.True(response.IsSuccessStatusCode);
//        Assert.Equal(200, (int)response.StatusCode);
//    }

//    [Theory]
//    [InlineData("/Event", "GET")]
//    [InlineData("/Announcement/All", "GET")]
//    [InlineData("/Announcement/1", "GET")]
//    [InlineData("/Announcement", "POST")]
//    [InlineData("/Announcement/1", "PUT")]
//    [InlineData("/Announcement/1", "DELETE")]
//    [InlineData("/Users", "GET")]
//    [InlineData("/Users", "POST")]
//    [InlineData("/Users", "PUT")]
//    [InlineData("/Users", "DELETE")]
//    public async Task GatewayRoutes_ShouldReturnExpectedHttpMethod(string route, string httpMethod)
//    {
//        // Act
//        var method = new HttpMethod(httpMethod);
//        var request = new HttpRequestMessage(method, route);
//        var response = await _httpClient.SendAsync(request);

//        // Assert
//        Assert.True(response.IsSuccessStatusCode);
//        Assert.Equal(200, (int)response.StatusCode);
//    }
//}
