using Moq;
using System.Net;
using LastSeenApplication;
using Moq.Protected;
using Newtonsoft.Json;
using Xunit;

public class ForthFeatureTest
{
    [Fact]
    public async Task GetPredictionOnlineUser_Success()
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
        string time = "2023-10-18-21:12:03";
        string id = "cbf0d80b-8532-070b-0df6-a0279e65d0b2";
        string tolerance = "0.82";
        string output = "{\"isOnline\":false,\"chancePercent\":0}";
        // Act
        var result = Program.GetPredictionOnlineUser(time, tolerance, id).Result;
        // Assert
        Assert.NotNull(result);
        Assert.Equal(output, result);
        
    }
    [Fact]
    public async Task GetPredictionOnlineUser_Null()
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
        string time = "2023-10-17-21:12:03";
        string id = "cbf0d80b-8532-070b-0df6-a0279e65d0b2";
        string tolerance = "0.82";
        // Act
        var result = Program.GetPredictionOnlineUser(time, tolerance, id).Result;
        // Assert
        Assert.Null(result);
        
    }   
}