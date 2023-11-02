using Moq;
using System.Net;
using LastSeenApplication;
using Moq.Protected;
using Newtonsoft.Json;
using Xunit;

public class ExamTest
{
    [Fact]
    public async Task GetAll_Success()
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
        

        // Act
        var result = Program.GetAllUsers().Result;
        
        // Assert
        Assert.NotNull(result);
        Assert.Contains("Doug93", result);
        Assert.Contains("a807e6f7-ec9c-f8a6-a6e4-43b8f36c78cc", result);

    }
}