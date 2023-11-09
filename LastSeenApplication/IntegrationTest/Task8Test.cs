using System.Net;
using LastSeenApplication;
using Moq;
using Moq.Protected;

namespace IntegrationTest;

public class Task8Test
{
    [Fact]
    public void GetAllReports()
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
        
        string jsonData = "[{\"name\":\"report-2023-10-30.json\",\"_report\":\"{\\\"Metrics\\\":{\\\"DailyAverage\\\":8,\\\"WeeklyAverage\\\":56,\\\"Total\\\":35,\\\"Min\\\":6,\\\"Max\\\":35},\\\"Users\\\":\\\"1fa8819c-1386-dc9b-840b-e7d0dc84f158\\\"}\"},{\"name\":\"report-2023-10-31.json\",\"_report\":\"{\\\"Metrics\\\":{\\\"DailyAverage\\\":8,\\\"WeeklyAverage\\\":56,\\\"Total\\\":35,\\\"Min\\\":35,\\\"Max\\\":35},\\\"Users\\\":\\\"1fa8819c-1386-dc9b-840b-e7d0dc84f158\\\"}\"},{\"name\":\"report-2023-11-01.json\",\"_report\":\"{\\\"Metrics\\\":{\\\"DailyAverage\\\":null,\\\"WeeklyAverage\\\":null,\\\"Total\\\":35,\\\"Min\\\":null,\\\"Max\\\":null},\\\"Users\\\":\\\"1fa8819c-1386-dc9b-840b-e7d0dc84f158\\\"}\"}]";


        // Act
        var result = Program.GetAllReports().Result;
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(jsonData, result);
    }
}