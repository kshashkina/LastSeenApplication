using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading;

class Program
{
    static void Main()
    {
        bool shouldRun = true;
        Dictionary<string, Dictionary<string, int>> day = new Dictionary<string, Dictionary<string, int>>();
        Dictionary<string, Dictionary<int, int>> week = new Dictionary<string, Dictionary<int, int>>();

        while (shouldRun)
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

                    foreach (var user in day)
                    {
                        foreach (var dayInfo in user.Value)
                        {
                            foreach (var weekInfo in week[user.Key])
                            {
                                var onlineUser = new OnlineUsersData
                                {
                                    userId = user.Key,
                                    wasTimeOnlineDay = dayInfo.Key,
                                    wasTimeOnlineTime = dayInfo.Value,
                                    wasTimeOnlineWeek = weekInfo.Key,
                                    wasTimeOnlineWeekTime = weekInfo.Value
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
                        }
                    }
                    Thread.Sleep(1000);
                    break;
                }

                foreach (var user in userData)
                {
                    string todayString = DateTime.Now.Date.ToString("yyyy-MM-dd");
                    int weekNumber = GetWeekNumber(DateTime.Now);
                    if (user.lastSeenDate == null)
                    {
                        if (day.ContainsKey(user.userId))
                        {
                            if (day[user.userId].ContainsKey(todayString))
                            {
                                day[user.userId][todayString]++;
                            }
                            else
                            {
                                day[user.userId][todayString] = 1;
                            }

                            if (week.ContainsKey(user.userId))
                            {
                                if (week[user.userId].ContainsKey(weekNumber))
                                {
                                    week[user.userId][weekNumber]++;
                                }
                                else
                                {
                                    week[user.userId][weekNumber] = 1;
                                }
                            }
                            else
                            {
                                week[user.userId] = new Dictionary<int, int> { { weekNumber, 1 } };
                            }
                        }
                        else
                        {
                            day[user.userId] = new Dictionary<string, int> { { todayString, 1 } };
                            week[user.userId] = new Dictionary<int, int> { { weekNumber, 1 } };
                        }
                    }
                }

                offset += userData.Length;
            }
        }
    }

    static int GetWeekNumber(DateTime date)
    {
        CultureInfo ciCurr = CultureInfo.CurrentCulture;
        int weekNum = ciCurr.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        return weekNum;
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
        public string wasTimeOnlineDay { get; set; }
        public int wasTimeOnlineTime { get; set; }
        public int wasTimeOnlineWeek { get; set; }
        public int wasTimeOnlineWeekTime { get; set; }
    }
}
