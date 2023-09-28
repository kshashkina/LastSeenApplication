﻿using Newtonsoft.Json;
using System;
using System.Net.Http;

namespace LastSeenApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var offset = 0;
            while (true)
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = client
                        .GetAsync(new Uri($"https://sef.podkolzin.consulting/api/users/lastSeen?offset={offset}"))
                        .Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonData = response.Content.ReadAsStringAsync().Result;
                        UserData userData = JsonConvert.DeserializeObject<UserData>(jsonData);
                        if (userData.data == null || userData.data.Length == 0)
                        {
                            return;
                        }

                        foreach (var user in userData.data)
                        {
                            RetrieveUserData(user);
                        }

                        offset += 20;
                    }
                    else
                    {
                        Console.WriteLine("Error: " + response.StatusCode);
                    }
                }
            }
        }

        public static void RetrieveUserData(User user)
        {
            string nickName = user.nickname;

            if (user.lastSeenDate == null)
            {
                Console.WriteLine($"{nickName} is online.");
            }
            else
            {
                DateTime now = DateTime.Now;
                DateTime givenDate = user.lastSeenDate.Value;
                TimeSpan difference = now - givenDate;
                string timeAgo = GetTimeAgoString(difference);

                Console.WriteLine($"{nickName} was online {timeAgo}");
            }
        }
        public static string GetTimeAgoString(TimeSpan difference)
        {
            if (difference.TotalSeconds <= 30)
            {
                return "just now";
            }
            else if (difference.TotalSeconds <= 60)
            {
                return "less than a minute ago";
            }
            else if (difference.TotalMinutes <= 59)
            {
                return "couple of minutes ago";
            }
            else if (difference.TotalMinutes <= 119)
            {
                return "an hour ago";
            }
            else if (difference.TotalMinutes <= 23 * 60)
            {
                return "today";
            }
            else if (difference.TotalMinutes <= 47 * 60)
            {
                return "yesterday";
            }
            else if (difference.TotalDays < 7)
            {
                return "this week";
            }
            else
            {
                return "long time ago";
            }
        }
    }
    public class UserData
    {
        public User[] data { get; set; }
    }

    public class User
    {
        public string nickname { get; set; }
        public DateTime? lastSeenDate { get; set; }
    }
}
