using System.Net;
using LastSeenApplication;
using Moq;
using Moq.Protected;
using Xunit.Sdk;

namespace IntegrationTest;

public class OutPutTest : IDisposable
{
    private StringWriter consoleOutput;
    private TextWriter originalConsoleOutput;

    public OutPutTest()
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
    public void OutPut()
    {
        var localization = new Localization();
        Console.SetIn(new StringReader("1"));
        Console.SetOut(consoleOutput);
        string language = localization.ChooseLanguage();
        Assert.Equal("en", language);
        var text = localization.Output(language);
        string expectedOutput = "What you want to do? \nHave a list of all users - 1 \nHave number of users at the exact time - 2\n Check if the user was online at the exact date - 3\nPrediction about amount of the users online - 4\nPrediction about user online - 5";
        Assert.Equal(expectedOutput, text);
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
        Assert.NotNull(result);
        Assert.Equal(2, result.Length);
        
        User userJohn = new User { nickname = "John", lastSeenDate = null };
        User userMary = new User { nickname = "Mary", lastSeenDate = DateTime.Now.AddMinutes(-10) };
        string formattedMessageJohn = localization.FormatUserData(userJohn, language);
        Assert.Equal("John is online.", formattedMessageJohn);
        string formattedMessageMary = localization.FormatUserData(userMary, language);
        Assert.Contains("minutes ago", formattedMessageMary);
    }
}

public class GetOnlineUsersCount : IDisposable
{
    private StringWriter consoleOutput;
    private TextWriter originalConsoleOutput;

    public GetOnlineUsersCount()
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
    public void GetOnlineUsersCount_Success()
    {
        var localization = new Localization();
        Console.SetIn(new StringReader("2"));
        Console.SetOut(consoleOutput);
        string language = localization.ChooseLanguage();
        Assert.Equal("uk", language);
        var text = localization.Output(language);
        string expectedOutput =
            "Що ви хочете зробити? \nОтримати список всіх користувачів - 1 \nОтримати кількість користувачів в точний час - 2" +
            "\n Перевірити, чи був користувач в мережі в точну дату - 3" +
            "\nПрогноз кількості користувачів онлайн - 4\nПрогноз користувачів онлайн - 5";
        Assert.Equal(expectedOutput, text);
        var mockHttpHandler = new Mock<HttpMessageHandler>();
        var httpClient = new HttpClient(mockHttpHandler.Object);
        mockHttpHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
            });
        string time = "2023-09-10-08:58:51";
        string count = "{\"usersOnline\":45}";
        var result = Program.GetOnlineUsersCount(time).Result;
        Assert.NotNull(result);
        Assert.Equal(count, result);
    }
}

public class GetUser: IDisposable
{
    private StringWriter consoleOutput;
    private TextWriter originalConsoleOutput;

    public GetUser()
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
    public void GetUser_Success()
    {
        var localization = new Localization();
        Console.SetIn(new StringReader("3"));
        Console.SetOut(consoleOutput);
        string language = localization.ChooseLanguage();
        Assert.Equal("de", language);
        var text = localization.Output(language);
        string expectedOutput =
            "Was möchten Sie tun? \nEine Liste aller Benutzer haben - 1 \nDie Anzahl der Benutzer zu einem bestimmten Zeitpunkt haben - 2" +
            "\n Überprüfen Sie, ob der Benutzer an einem bestimmten Datum online war - 3" +
            "\nPrognose zur Anzahl der Benutzer online - 4\nPrognose für Benutzer online - 5";
        Assert.Equal(expectedOutput, text);
        // Arrange
        var mockHttpHandler = new Mock<HttpMessageHandler>();
        var httpClient = new HttpClient(mockHttpHandler.Object);
        mockHttpHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
            });
        string time = "2023-10-11-21:12:03";
        string id = "bb367131-ec06-3d69-a861-eeca3f9cc88d";
        string output = "{\"isOnline\":\"false\",\"lastSeen\":\"11.10.2023 21:11:12\"}";
        // Act
        var result = Program.GetUserDate(time, id).Result;
        // Assert
        Assert.NotNull(result);
        Assert.Equal(output, result);
    }
}

public class OnlinePrediction : IDisposable
{
    private StringWriter consoleOutput;
    private TextWriter originalConsoleOutput;

    public OnlinePrediction()
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
    public void GetUser_Success()
    {
        var localization = new Localization();
        Console.SetIn(new StringReader("4"));
        Console.SetOut(consoleOutput);
        string language = localization.ChooseLanguage();
        Assert.Equal("fr", language);
        var text = localization.Output(language);
        string expectedOutput =
            "Que souhaitez-vous faire ? \nObtenir la liste de tous les utilisateurs - 1 \nObtenir le nombre d'utilisateurs à un moment précis - 2\n" +
            " Vérifier si l'utilisateur était en ligne à une date précise - 3\nPrévision sur le nombre d'utilisateurs en ligne - " +
            "4\nPrévision sur les utilisateurs en ligne - 5";
        Assert.Equal(expectedOutput, text);
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
}

public class UserPrediction : IDisposable
{
    private StringWriter consoleOutput;
    private TextWriter originalConsoleOutput;

    public UserPrediction()
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
    public void GetUser_Success()
    {
        var localization = new Localization();
        Console.SetIn(new StringReader("5"));
        Console.SetOut(consoleOutput);
        string language = localization.ChooseLanguage();
        Assert.Equal("en", language);
        var text = localization.Output(language);
        string expectedOutput =
            "What you want to do? \nHave a list of all users - 1 \nHave number of users at the exact time -" +
            " 2\n Check if the user was online at the exact date - 3\nPrediction about amount of the users online - 4\n" +
            "Prediction about user online - 5";
        Assert.Equal(expectedOutput, text);
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
}
