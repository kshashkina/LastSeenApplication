using System.Net;
using LastSeenApplication;
using Moq;
using Moq.Protected;

namespace IntegrationTest;

public class Task2 
{
    [Fact]
    public void OutPut()
    {
        var localization = new Localization();
        string resultEN = localization.LanguageKey(1);
        var text = localization.Output(resultEN);
        string expectedOutput = "What you want to do? \nHave a list of all users - 1 \nHave number of users at the exact time - 2\n Check if the user was online at the exact date - 3\nPrediction about amount of the users online - 4\nPrediction about user online - 5";
        
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
        var result =  Program.FetchUserData(0, httpClient);
        
        
        User userJohn = new User { nickname = "John", lastSeenDate = null };
        User userMary = new User { nickname = "Mary", lastSeenDate = DateTime.Now.AddMinutes(-10) };
        string formattedMessageJohn = localization.FormatUserData(userJohn, resultEN);
        string formattedMessageMary = localization.FormatUserData(userMary, resultEN);


        Assert.Equal("en", resultEN);
        Assert.Equal(expectedOutput, text);
        Assert.NotNull(result);
        Assert.Equal(2, result.Length);
        Assert.Equal("John is online.", formattedMessageJohn);
        Assert.Contains("minutes ago", formattedMessageMary);
    }
}