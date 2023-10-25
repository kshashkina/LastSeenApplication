using Moq;
using System.Net;
using LastSeenApplication;
using Moq.Protected;
using Newtonsoft.Json;
using Xunit;

public class UserDataFetcherTests
{
    [Fact]
    public async Task FetchUserData_WithValidResponse_ReturnsUserDataArray()
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
                Content = new StringContent(@"{ ""data"": [ { ""name"": ""User1"" }, { ""name"": ""User2"" } ] }")
            });

        // Act
        var result =  Program.FetchUserData(0, httpClient);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Length);
    }

    [Fact]
    public async Task FetchUserData_WithInvalidResponse_ReturnsNull()
    {
        // Arrange
        var mockHttpHandler = new Mock<HttpMessageHandler>();
        var httpClient = new HttpClient(mockHttpHandler.Object);
        mockHttpHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError
            });

        // Act
        var result =  Program.FetchUserData(0, httpClient);

        // Assert
        Assert.Null(result);
    }
}