using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Globalization;

public class User
{
    public DateTime? LastSeenDate { get; set; }
    public string UserId { get; set; }
    public string Nickname { get; set; }
}

public class OnlineUsersData
{
    public string UserId { get; set; }
    public string LastSeen { get; set; }
    public string Nickname { get; set; }
}

public class Program
{
    static void Main()
    {
        // First set of code
        bool shouldRun1 = true;
        Dictionary<string, int> onlineUsersCount1 = new Dictionary<string, int>();

        while (shouldRun1)
        {
            int offset = 0;
            int onlineUsers = 0;
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

                    foreach (var user in onlineUsersCount1)
                    {
                        var onlineUser = new OnlineUsersData
                        {
                            UserId = user.Key,
                            LastSeen = DateTime.Now.ToString(),
                            Nickname = user.Value.ToString()
                        };
                        SaveOnlineUserData(onlineUser);
                    }
                    Thread.Sleep(1000);
                    break;
                }

                foreach (var user in userData)
                {
                    if (user.LastSeenDate == null)
                    {
                        if (onlineUsersCount1.ContainsKey(user.UserId))
                        {
                            onlineUsersCount1[user.UserId]++;
                        }
                        else
                        {
                            onlineUsersCount1[user.UserId] = 1;
                        }
                    }
                }

                offset += userData.Length;
            }
        }

        // Second set of code
        bool shouldRun2 = true;
        Dictionary<Tuple<string, string, string>, int> onlineUsersCount2 = new Dictionary<Tuple<string, string, string>, int>();

        while (shouldRun2)
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

                    foreach (var user in onlineUsersCount2)
                    {
                        var userId = user.Key.Item1;
                        var weekNumber = user.Key.Item2;
                        var day = user.Key.Item3;
                        var onlineUser = new OnlineUsersData
                        {
                            UserId = userId,
                            LastSeen = DateTime.Now.ToString(),
                            Nickname = user.Value.ToString()
                        };
                        SaveOnlineUserData(onlineUser);
                    }
                    Thread.Sleep(1000);
                    break;
                }

                foreach (var user in userData)
                {
                    if (user.LastSeenDate != null)
                    {
                        string userId = user.UserId;
                        string weekNumber = GetWeekOfYear(user.LastSeenDate.Value).ToString();
                        string day = user.LastSeenDate.Value.ToString("yyyy-MM-dd");

                        var key = new Tuple<string, string, string>(userId, weekNumber, day);
                        if (!onlineUsersCount2.ContainsKey(key))
                        {
                            onlineUsersCount2[key] = 0;
                        }

                        onlineUsersCount2[key]++;
                    }
                }

                offset += userData.Length;
            }
        }

        // Third set of code
        bool shouldRun3 = true;

        while (shouldRun3)
        {
            int offset = 0;
            while (true)
            {
                var userData = FetchUserData(offset);
                if (userData == null || userData.Length == 0)
                {
                    return;
                }
                foreach (var user in userData)
                {
                    var onlineUser = new OnlineUsersData
                    {
                        UserId = user.UserId,
                        LastSeen = user.LastSeenDate.HasValue ? user.LastSeenDate.ToString() : DateTime.Now.ToString(),
                        Nickname = user.Nickname
                    };
                    SaveOnlineUserData(onlineUser);
                }

                offset += 20;
            }
            Thread.Sleep(10000);
        }
    }

    static void SaveOnlineUserData(OnlineUsersData onlineUser)
    {
        string json = JsonConvert.SerializeObject(onlineUser);
        string filePath = "online.json";
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
                return JsonConvert.DeserializeObject<UserData>(jsonData)?.Data;
            }
            else
            {
                Console.WriteLine("Error: " + response.StatusCode);
                return null;
            }
        }
    }

    static int GetWeekOfYear(DateTime date)
    {
        var ci = CultureInfo.CurrentCulture;
        var cal = ci.Calendar;
        var calWeekRule = ci.DateTimeFormat.CalendarWeekRule;
        var calFirstDayOfWeek = ci.DateTimeFormat.FirstDayOfWeek;
        return cal.GetWeekOfYear(date, calWeekRule, calFirstDayOfWeek);
    }
}

public class UserData
{
    public User[] Data { get; set; }
}
