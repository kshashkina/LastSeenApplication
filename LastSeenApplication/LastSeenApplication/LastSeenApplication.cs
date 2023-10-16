﻿using System;
using Newtonsoft.Json;

namespace LastSeenApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Localization localization = new Localization();
            var language = localization.ChooseLanguage();
            localization.Output(language);
            int input = Convert.ToInt32(Console.ReadLine());
            switch (input)
            {
                case 1:
                    RunLastSeenApplication(0, language);
                    break;
                case 2: 
                    GetOnlineUsersCount();
                    break;
                case 3:
                    GetUserDate();
                    break;
                case 4:
                    GetPredictionOnline();
                    break;
                case 5:
                    GetPredictionOnlineUser();
                    break;
            }
        }
        
        static async Task GetOnlineUsersCount()
        {
            Console.WriteLine("Write your date:");
            var date = Console.ReadLine();
            string apiUrl = $"http://localhost:5169/api/stats/users?date={date}";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = client.GetAsync(new Uri(apiUrl)).Result;

                    if (response.IsSuccessStatusCode)
                    {
                         Console.WriteLine(await response.Content.ReadAsStringAsync());

                    }
                    else
                    {
                        Console.WriteLine($"Error: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }
        
        static async Task GetUserDate()
        {
            Console.WriteLine("Write your date:");
            var date = Console.ReadLine();
            Console.WriteLine("Write user id:");
            var id = Console.ReadLine();
            string apiUrl = $"http://localhost:5130/api/stats/user?date={date}&userId={id}";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = client.GetAsync(new Uri(apiUrl)).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine(await response.Content.ReadAsStringAsync());

                    }
                    else
                    {
                        Console.WriteLine($"Error: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }
        
        static async Task GetPredictionOnline()
        {
            Console.WriteLine("Write your date:");
            var date = Console.ReadLine();
            string apiUrl = $"http://localhost:5221/api/prediction/user?date={date}";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = client.GetAsync(new Uri(apiUrl)).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine(await response.Content.ReadAsStringAsync());

                    }
                    else
                    {
                        Console.WriteLine($"Error: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }
        
        static async Task GetPredictionOnlineUser()
        {
            Console.WriteLine("Write your date:");
            var date = Console.ReadLine();
            Console.WriteLine("Write user id:");
            var id = Console.ReadLine();
            Console.WriteLine("Write tolerance:");
            var tolerance = Console.ReadLine();
            string apiUrl = $"http://localhost:5176/api/prediction/user?date={date}&tolerance={tolerance}&userId={id}";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = client.GetAsync(new Uri(apiUrl)).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine(await response.Content.ReadAsStringAsync());

                    }
                    else
                    {
                        Console.WriteLine($"Error: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }

        public static void RunLastSeenApplication(int startingOffset, string language)
        {
            int offset = startingOffset;
            Localization localization = new Localization();

            while (true)
            {
                var userData = FetchUserData(offset);
                if (userData == null || userData.Length == 0)
                {
                    return;
                }

                foreach (var user in userData)
                {
                    var formattedData = localization.FormatUserData(user, language);
                    Console.WriteLine(formattedData);
                }

                offset += userData.Length; 
            }
        }
        public static User[] FetchUserData(int offset, HttpClient client = null)
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
