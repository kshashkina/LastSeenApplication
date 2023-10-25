using Moq;
using System.Net;
using LastSeenApplication;
using Moq.Protected;
using Newtonsoft.Json;
using Xunit;

public class SecondFeatureFetcherTask4Test
{
    [Fact]
    public async Task GetAverageTimeOnline_Success()
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
        string id = "8b0b5db6-19d6-d777-575e-915c2a77959a";
        string count = "{\"averageWeek\":26,\"averageDay\":3}";

        // Act
        var result = Program.GetAverageTimeForUser(id).Result;
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(count, result);
        
    }

    [Fact]
    public async Task GetAverageTimeOnline_Null()
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
        string id = "8b0b5db6-19d6-d777-575e-915c2a779597";


        // Act
        var result = Program.GetAverageTimeForUser(id).Result;
        
        // Assert
        Assert.Null(result);
    }
}