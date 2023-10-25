using Moq;
using System.Net;
using LastSeenApplication;
using Moq.Protected;
using Newtonsoft.Json;
using Xunit;

public class ThirdFeatureFetcherTask4Test
{
    [Fact]
    public async Task Delete_Success()
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
        string id = "cbf0d80b-8532-070b-0df6-a0279e65d0b2";
        string count = "{\"id\":\"cbf0d80b-8532-070b-0df6-a0279e65d0b2\"}";

        // Act
        var result = Program.GetTotalTimeForUser(id).Result;
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(count, result);
        
    }

    [Fact]
    public async Task Delete_Null()
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
        string id = "cbf0d80b-8532-070b-0df6-a0279e6580b2";


        // Act
        var result = Program.GetOnlineUsersCount(id).Result;
        
        // Assert
        Assert.Null(result);
    }
}