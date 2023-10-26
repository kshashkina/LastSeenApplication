using System.Net;
using LastSeenApplication;
using Moq;
using Moq.Protected;
using Xunit.Sdk;

namespace IntegrationTest;

public class Task3
{
    [Fact]
    public void GetOnlineUsersCount_Success()
    {
        var localization = new Localization();
        string resultUK = localization.LanguageKey(2);
        var text = localization.Output(resultUK);
        string expectedOutput =
            "Що ви хочете зробити? \nОтримати список всіх користувачів - 1 \nОтримати кількість користувачів в точний час - 2" +
            "\n Перевірити, чи був користувач в мережі в точну дату - 3" +
            "\nПрогноз кількості користувачів онлайн - 4\nПрогноз користувачів онлайн - 5";
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
        
        
        Assert.Equal("uk", resultUK);
        Assert.Equal(expectedOutput, text);
        Assert.NotNull(result);
        Assert.Equal(count, result);
    }
    [Fact]
    public void GetUser_Success()
    {
        var localization = new Localization();
        string resultDE = localization.LanguageKey(3);
        var text = localization.Output(resultDE);
        string expectedOutput =
            "Was möchten Sie tun? \nEine Liste aller Benutzer haben - 1 \nDie Anzahl der Benutzer zu einem bestimmten Zeitpunkt haben - 2" +
            "\n Überprüfen Sie, ob der Benutzer an einem bestimmten Datum online war - 3" +
            "\nPrognose zur Anzahl der Benutzer online - 4\nPrognose für Benutzer online - 5";
        
        
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
        Assert.Equal("de", resultDE);
        Assert.Equal(expectedOutput, text);
        Assert.NotNull(result);
        Assert.Equal(output, result);
    }
    [Fact]
    public void GetUserPrediction_Success()
    {
        var localization = new Localization();
        string resultFR = localization.LanguageKey(4);
        var text = localization.Output(resultFR);
        string expectedOutput =
            "Que souhaitez-vous faire ? \nObtenir la liste de tous les utilisateurs - 1 \nObtenir le nombre d'utilisateurs à un moment précis - 2\n" +
            " Vérifier si l'utilisateur était en ligne à une date précise - 3\nPrévision sur le nombre d'utilisateurs en ligne - " +
            "4\nPrévision sur les utilisateurs en ligne - 5";
        
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
        Assert.Equal("fr", resultFR);
        Assert.Equal(expectedOutput, text);
        Assert.NotNull(result);
        Assert.Equal(count, result);
    }
    [Fact]
    public void GetUserPredictionOnline_Success()
    {
        var localization = new Localization();
        string resultEN = localization.LanguageKey(5);
        var text = localization.Output(resultEN);
        string expectedOutput =
            "What you want to do? \nHave a list of all users - 1 \nHave number of users at the exact time -" +
            " 2\n Check if the user was online at the exact date - 3\nPrediction about amount of the users online - 4\n" +
            "Prediction about user online - 5";
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
        Assert.Equal("en", resultEN);
        Assert.Equal(expectedOutput, text);
        Assert.NotNull(result);
        Assert.Equal(output, result);
    }
}

