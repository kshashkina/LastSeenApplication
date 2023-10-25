using Moq;
using System.Net;
using LastSeenApplication;
using Moq.Protected;
using Newtonsoft.Json;
using Xunit;

public class SecondFeatureTest
{
    [Fact]
    public async Task GetUserDate_Success()
    {
        // Arrange
        var mockHttpHandler = new Mock<HttpMessageHandler>();
        var httpClient = new HttpClient(mockHttpHandler.Object);
        mockHttpHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
            });
        string time = "2023-10-11-21:12:03";
        string id = "bb367131-ec06-3d69-a861-eeca3f9cc88d";
        string output = "{\"isOnline\":false,\"lastSeen\":\"11.10.2023 21:11:12\"}";
        // Act
        var result = Program.GetUserDate(time, id).Result;
        // Assert
        Assert.NotNull(result);
        Assert.Equal(output, result);
        
    }
    [Fact]
    public async Task GetUserDate_Null()
    {
        // Arrange
        var mockHttpHandler = new Mock<HttpMessageHandler>();
        var httpClient = new HttpClient(mockHttpHandler.Object);
        mockHttpHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
            });
        string time = "2023-10-11-23:12:03";
        string id = "bb367131-ec06-3d69-a861-eeca3f9cc88d";
        // Act
        var result = Program.GetUserDate(time, id).Result;
        // Assert
        Assert.Null(result);
        
    }
}
