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
        var localization = new Localization();
        string resultEN = localization.LanguageKey(1);
        var text = localization.Output(resultEN);
        string expectedOutput = "What you want to do?\nHave a list of all users - 1\nHave the number of users at the exact time - 2\nCheck if the user was online at the exact date - 3\nPrediction about the number of users online - 4\nPrediction about a user being online - 5\nTotal amount of time online for a user - 6\nAverage time for a user - 7\nDisplay deleted user - 8\nPost report - 9\nGet report - 10";
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
        Assert.Equal("en", resultEN);
        Assert.Equal(expectedOutput, text);
        Assert.NotNull(result);
        Assert.Contains("Doug93", result);
        Assert.Contains("a807e6f7-ec9c-f8a6-a6e4-43b8f36c78cc", result);

    }
}