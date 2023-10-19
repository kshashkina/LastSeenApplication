using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Globalization;

bool shouldRun = true;
Dictionary<Tuple<string, string>, int> onlineUsersCount = new Dictionary<Tuple<string, string>, int>();

while (shouldRun)
{
    int offset = 0;
    while (true)
    {
        var userData = FetchUserData(offset);
        if (userData == null || userData.Length == 0)
        {
            string filePath = "online.json";
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            foreach (var user in onlineUsersCount)
            {
                var userId = user.Key.Item1;
                var weekNumber = user.Key.Item2;
                var onlineUser = new OnlineUsersData
                {
                    userId = userId,
                    weekNumber = weekNumber,
                    wasTimeOnline = user.Value,
                };
                string json = JsonConvert.SerializeObject(onlineUser);
                if (File.Exists(filePath))
                {
                    string existingContent = File.ReadAllText(filePath);
                    existingContent += '\n' + json;
                    File.WriteAllText(filePath, existingContent);
                }
                else
                {
                    File.WriteAllText(filePath, json);
                }
            }
            Thread.Sleep(1000);
            break;
        }

        foreach (var user in userData)
        {
            if (user.lastSeenDate != null)
            {
                string userId = user.userId;
                string weekNumber = GetWeekOfYear(user.lastSeenDate.Value).ToString();

                var key = new Tuple<string, string>(userId, weekNumber);

                if (!onlineUsersCount.ContainsKey(key))
                {
                    onlineUsersCount[key] = 0;
                }

                onlineUsersCount[key]++;
            }
        }

        offset += userData.Length;
    }
}

// Остальной код остается без изменений

static int GetWeekOfYear(DateTime date)
{
    var ci = CultureInfo.CurrentCulture;
    var cal = ci.Calendar;
    var calWeekRule = ci.DateTimeFormat.CalendarWeekRule;
    var calFirstDayOfWeek = ci.DateTimeFormat.FirstDayOfWeek;
    return cal.GetWeekOfYear(date, calWeekRule, calFirstDayOfWeek);
}

static User[] FetchUserData(int offset, HttpClient client = null)
{
    if (client == null)
    {
        client = new HttpClient();
    }

    using (client)
    {
        HttpResponseMessage response = client
            .GetAsync(new Uri($"https://sef.podkolzin.consulting/api/users/lastSeen?offset={offset}"))
            .Result;

        if (response.IsSuccessStatusCode)
        {
            string jsonData = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<UserData>(jsonData)?.data;
        }
        else
        {
            Console.WriteLine("Error: " + response.StatusCode);
            return null;
        }
    }
}

public class User
{
    public DateTime? lastSeenDate { get; set; }
    public string userId { get; set; }
}

public class UserData
{
    public User[] data { get; set; }
}

public class OnlineUsersData
{
    public string userId { get; set; }
    public string weekNumber { get; set; }
    public int wasTimeOnline { get; set; }
}
