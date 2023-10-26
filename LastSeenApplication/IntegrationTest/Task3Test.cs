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
            "Що ви хочете зробити?\n" +
            "Отримати список всіх користувачів - 1\n" +
            "Отримати кількість користувачів в точний час - 2\n" +
            "Перевірити, чи був користувач в мережі в точну дату - 3\n" +
            "Прогноз кількості користувачів онлайн - 4\n" +
            "Прогноз користувачів онлайн - 5\n" +
            "Загальна кількість часу онлайн для користувача - 6\n" +
            "Середній час для користувача - 7\n" +
            "Показати видаленого користувача - 8";
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
            "Was möchten Sie tun?\n" +
            "Eine Liste aller Benutzer haben - 1\n" +
            "Die Anzahl der Benutzer zu einem bestimmten Zeitpunkt haben - 2\n" +
            "Überprüfen Sie, ob der Benutzer an einem bestimmten Datum online war - 3\n" +
            "Prognose zur Anzahl der Benutzer online - 4\n" +
            "Prognose für Benutzer online - 5\n" +
            "Gesamte Online-Zeit für einen Benutzer - 6\n" +
            "Durchschnittliche Zeit für einen Benutzer - 7\n" +
            "Gelöschten Benutzer anzeigen - 8";
        
        
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
            "Que souhaitez-vous faire ?\n" +
            "Obtenir la liste de tous les utilisateurs - 1\n" +
            "Obtenir le nombre d'utilisateurs à un moment précis - 2\n" +
            "Vérifier si l'utilisateur était en ligne à une date précise - 3\n" +
            "Prévision sur le nombre d'utilisateurs en ligne - 4\n" +
            "Prévision sur les utilisateurs en ligne - 5\n" +
            "Temps total en ligne pour un utilisateur - 6\n" +
            "Temps moyen pour un utilisateur - 7\n" +
            "Afficher l'utilisateur supprimé - 8";
        
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
            "What you want to do?\n" +
            "Have a list of all users - 1\n" +
            "Have the number of users at the exact time - 2\n" +
            "Check if the user was online at the exact date - 3\n" +
            "Prediction about the number of users online - 4\n" +
            "Prediction about a user being online - 5\n" +
            "Total amount of time online for a user - 6\n" +
            "Average time for a user - 7\n" +
            "Display deleted user - 8";
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

