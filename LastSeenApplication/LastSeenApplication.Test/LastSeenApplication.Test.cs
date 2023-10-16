using Moq;
using System.Net;
using LastSeenApplication;
using Moq.Protected;
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


public class LocalizationTests : IDisposable
{
    private StringWriter consoleOutput;
    private TextWriter originalConsoleOutput;

    public LocalizationTests()
    {
        // Initialize and capture the original Console.Out
        consoleOutput = new StringWriter();
        originalConsoleOutput = Console.Out;
    }

    public void Dispose()
    {
        // Restore the original Console.Out and dispose of the StringWriter
        Console.SetOut(originalConsoleOutput);
        consoleOutput.Dispose();
    }

    [Fact]
    public void ChooseLanguage_ValidChoice_ReturnsLanguageCode()
    {
        // Arrange
        var localization = new Localization();

        // Simulate user input
        Console.SetIn(new StringReader("1"));

        // Redirect Console.Out to the StringWriter
        Console.SetOut(consoleOutput);

        // Act
        string language = localization.ChooseLanguage();

        // Assert
        Assert.Equal("en", language);

        // Reset Console.Out
        Console.SetOut(originalConsoleOutput);
    }

    [Fact]
    public void ChooseLanguage_InvalidChoice_ReturnsDefaultLanguage()
    {
        // Arrange
        var localization = new Localization();

        // Simulate user input
        Console.SetIn(new StringReader("5"));

        // Redirect Console.Out to the StringWriter
        Console.SetOut(consoleOutput);

        // Act
        string language = localization.ChooseLanguage();

        // Assert
        Assert.Equal("en", language);

        // Reset Console.Out
        Console.SetOut(originalConsoleOutput);
    }

    [Fact]
    public void Output_WithLanguageCode_ReturnsCorrectOutput()
    {
        // Arrange
        var localization = new Localization();
        string language = "en";

        // Redirect Console.Out to the StringWriter
        Console.SetOut(consoleOutput);

        // Act
        localization.Output(language);

        // Assert
        string expectedOutput = "What you want to do? \nHave a list of all users - 1 \nHave number of users at the exact time - 2\n Check if the user was online at the exact date - 3\nPrediction about amount of the users online - 4\nPrediction about user online - 5\r\n";
        Assert.Equal(expectedOutput, consoleOutput.ToString());

        // Reset Console.Out
        Console.SetOut(originalConsoleOutput);
    }

    [Fact]
    public void FormatUserData_OnlineUser_ReturnsOnlineMessage()
    {
        // Arrange
        var localization = new Localization();
        string language = "en";
        User user = new User { nickname = "John", lastSeenDate = null };

        // Act
        string formattedMessage = localization.FormatUserData(user, language);

        // Assert
        Assert.Equal("John is online.", formattedMessage);
    }

    [Fact]
    public void FormatUserData_OfflineUser_ReturnsTimeAgoMessage()
    {
        // Arrange
        var localization = new Localization();
        string language = "en";
        User user = new User { nickname = "John", lastSeenDate = DateTime.Now.AddMinutes(-10) };

        // Act
        string formattedMessage = localization.FormatUserData(user, language);

        // Assert
        // You can assert based on the expected localized time ago message
        Assert.Contains("minutes ago", formattedMessage);
    }
}