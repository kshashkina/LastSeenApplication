using System.Net;
using LastSeenApplication;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using OnlineApi;
using Xunit;
using Xunit.Sdk;

namespace IntegrationTest;

public class E2ETests 
{
    [Fact]
    public async void E2ETest()
    {
        var localization = new Localization();
        string resultEN = localization.LanguageKey(1);
        var text = localization.Output(resultEN);
        string expectedOutput = "What you want to do?\n" +
                                "Have a list of all users - 1\n" +
                                "Have the number of users at the exact time - 2\n" +
                                "Check if the user was online at the exact date - 3\n" +
                                "Prediction about the number of users online - 4\n" +
                                "Prediction about a user being online - 5\n" +
                                "Total amount of time online for a user - 6\n" +
                                "Average time for a user - 7\n" +
                                "Display deleted user - 8\n" +
                                "Post report - 9\n" + 
                                "Get report - 10";
        var httpClient = new HttpClient();
        var result =  Program.FetchUserData(0, httpClient);
        var resultOnlineCount = await Program.GetOnlineUsersCount("2023-19-10-21:33:38");
        var resultIsUserOnline =
            await Program.GetUserDate("2023-10-11-21:12:03", "de5b8815-1689-7c78-44e1-33375e7e2931");
        var resultOnlineCountPrediction = await Program.GetPredictionOnline("2023-26-10-21:32:28");
        var resultIsUserOnlinePrediction = await Program.GetPredictionOnlineUser("2023-10-18-21:12:03", "0.82",
            "de5b8815-1689-7c78-44e1-33375e7e2931");
        var resultTotalTime = await Program.GetTotalTimeForUser("938a6656-0b54-6a9c-76a2-bbfac8f3de81");
        var resultAverageTime = await Program.GetAverageTimeForUser("938a6656-0b54-6a9c-76a2-bbfac8f3de81");
        var resultDeleteUser = await Program.DeleteUser("b9cd6eac-1ff3-fa09-703e-e09241c4b33c");
        var allUsers = await Program.GetAllUsers();
        
        Assert.Equal("en", resultEN);
        Assert.Equal(expectedOutput, text);
        Assert.NotNull(result);
        Assert.Equal(20, result.Length);
        Assert.Equal("{\"OnlineUsersCount\":53}", resultOnlineCount);
        Assert.Equal("{\"isOnline\":false,\"lastSeen\":\"11.10.2023 21:07:52\"}", resultIsUserOnline);
        Assert.Equal("{\"OnlineUsersCount\":57}", resultOnlineCountPrediction);
        Assert.Equal("{\"isOnline\":false,\"chancePercent\":0}", resultIsUserOnlinePrediction);
        Assert.Equal("{\"usersOnline\":42}",resultTotalTime);
        Assert.Equal("{\"averageWeek\":22,\"averageDay\":3}",resultAverageTime);
        Assert.Equal("{\"id\":\"b9cd6eac-1ff3-fa09-703e-e09241c4b33c\"}",resultDeleteUser);
        Assert.NotNull(allUsers);
        Assert.Contains("Doug93", allUsers);
        Assert.Contains("a807e6f7-ec9c-f8a6-a6e4-43b8f36c78cc", allUsers);

    }
}
