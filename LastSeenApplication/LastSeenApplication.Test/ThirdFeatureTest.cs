using Moq;
using System.Net;
using LastSeenApplication;
using Moq.Protected;
using Newtonsoft.Json;
using Xunit;


public class ThirdFeatureTest
{
    [Fact]
    public async Task GetPredictionOnline_Success()
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
        string time = "2023-09-17-08:59:14";
        string count = "{\"usersOnline\":47}";

        // Act
        var result = Program.GetPredictionOnline(time).Result;
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(count, result);
        
    }
    [Fact]
    public async Task GetPredictionOnline_Null()
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
        string time = "2023-09-16-08:59:14";

        // Act
        var result = Program.GetPredictionOnline(time).Result;
        
        // Assert
        Assert.Null(result);
    }
}