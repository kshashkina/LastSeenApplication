using System.Net;
using LastSeenApplication;
using Moq;
using Moq.Protected;

namespace IntegrationTest;

public class Task5
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
        string report = "report";
        string from = "2023-10-30";
        string to = "2023-10-31";
        
        string jsonReport = 
            @"{""report"":{""5ed89705-d3c6-6dea-880c-82ad5c185285"":{""dailyAverage"":7,""weeklyAverage"":52,""total"":10,""min"":5,""max"":5},""1fa8819c-1386-dc9b-840b-e7d0dc84f158"":{""dailyAverage"":8,""weeklyAverage"":56,""total"":70,""min"":35,""max"":35}}}";


        // Act
        var result = Program.GetReport(report, from,to).Result;
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(jsonReport, result);
    }
}