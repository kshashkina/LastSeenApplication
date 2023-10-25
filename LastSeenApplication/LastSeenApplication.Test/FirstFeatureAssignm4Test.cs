using Moq;
using System.Net;
using LastSeenApplication;
using Moq.Protected;
using Newtonsoft.Json;
using Xunit;

public class FirstFeatureFetcherTask4Test
{
    [Fact]
    public async Task GetTotalTimeOnline_Success()
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
        string id = "938a6656-0b54-6a9c-76a2-bbfac8f3de81";
        string count = "{\"usersOnline\":42}";

        // Act
        var result = Program.GetTotalTimeForUser(id).Result;
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(count, result);
        
    }

    [Fact]
    public async Task GetTotalTimeOnline_Null()
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
        string id = "938a6656-0b54-6a9c-76a2-bbfac8f3de21";


        // Act
        var result = Program.GetOnlineUsersCount(id).Result;
        
        // Assert
        Assert.Null(result);
    }
}