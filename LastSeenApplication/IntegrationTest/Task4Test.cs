using System.Net;
using LastSeenApplication;
using Moq;
using Moq.Protected;

namespace IntegrationTest;

public class Task4
{
    [Fact]
    public void GetTotalTime()
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
        string id = "938a6656-0b54-6a9c-76a2-bbfac8f3de81";
        string count = "{\"usersOnline\":42}";

        // Act
        var result = Program.GetTotalTimeForUser(id).Result;
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(count, result);
    }

    [Fact]
    public void GetAverage()
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
        string id = "8b0b5db6-19d6-d777-575e-915c2a77959a";
        string count = "{\"averageWeek\":26,\"averageDay\":3}";

        // Act
        var result = Program.GetAverageTimeForUser(id).Result;
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(count, result);
    }

    [Fact]
    public void DeleteUser()
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
        string id = "cbf0d80b-8532-070b-0df6-a0279e65d0b2";
        string count = "{\"id\":\"cbf0d80b-8532-070b-0df6-a0279e65d0b2\"}";

        // Act
        var result = Program.GetTotalTimeForUser(id).Result;
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(count, result);
    }
}