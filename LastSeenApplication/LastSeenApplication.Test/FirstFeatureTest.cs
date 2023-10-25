using Moq;
using System.Net;
using LastSeenApplication;
using Moq.Protected;
using Newtonsoft.Json;
using Xunit;

public class FirstFeatureFetcherTest
{
    [Fact]
    public async Task GetOnlineUsersCount_Success()
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
        string time = "2023-09-10-08:58:51";
        string count = "{\"usersOnline\":45}";

        // Act
        var result = Program.GetOnlineUsersCount(time).Result;
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(count, result);
        
    }

    [Fact]
    public async Task GetOnlineUsersCount_Null()
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
        string time = "2023-09-16-08:58:51";
        string count = "{\"usersOnline\":null}";


        // Act
        var result = Program.GetOnlineUsersCount(time).Result;
        
        // Assert
        Assert.Equal(count, result);
    }
}