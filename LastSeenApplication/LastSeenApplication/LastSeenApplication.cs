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
                    var date = localization.FirstFeatureTranslation(language);
                    GetOnlineUsersCount(date);
                    break;
                case 3:
                    var dateUser = "";
                    var id = "";
                    (dateUser, id) = localization.SecondFeatureTranslation(language);
                    GetUserDate(dateUser, id);
                    break;
                case 4:
                    var datePrediction = localization.FirstFeatureTranslation(language);
                    GetPredictionOnline(datePrediction);
                    break;
                case 5:
                    var datePredictionUser = "";
                    var idPredictionUser = "";
                    var tolerance = "";
                    (datePrediction, idPredictionUser, tolerance) = localization.ForthFeatureTranslation(language);
                    GetPredictionOnlineUser(datePredictionUser, tolerance, idPredictionUser);
                    break;
                case 6:
                    Console.WriteLine("Enter your id");
                    var idTotal = Console.ReadLine();
                    GetTotalTimeForUser(idTotal);
                    break;
                case 7:
                    Console.WriteLine("Enter your id");
                    var idAverage = Console.ReadLine();
                    GetAverageTimeForUser(idAverage);
                    break;
            }
        }
        
        public static async Task<string> GetOnlineUsersCount(string date)
        {
            string apiUrl = $"http://localhost:5169/api/stats/users?date={date}";

            Loader loader = new Loader();
            var result = await loader.Load(apiUrl);
            return result;
        }
        
        public static async Task<string> GetUserDate(string date, string id)
        {
            string apiUrl = $"http://localhost:5169/api/stats/user?date={date}&userId={id}";
            Loader loader = new Loader();
            var result = await loader.Load(apiUrl);
            return result;
        }
        
        public static async Task<string> GetPredictionOnline(string date)
        {
            string apiUrl = $"http://localhost:5169/api/prediction/user/average?date={date}";

            Loader loader = new Loader();
            var result = await loader.Load(apiUrl);
            return result;
        }
        
        public static async Task<string> GetPredictionOnlineUser(string date, string tolerance, string id)
        {
            string apiUrl = $"http://localhost:5169/api/prediction/user/status?date={date}&tolerance={tolerance}&userId={id}";

            Loader loader = new Loader();
            var result = await loader.Load(apiUrl);
            return result;
        }

        public static async Task<string> GetTotalTimeForUser(string id)
        {
            string apiUrl = $"http://localhost:5169/api/stats/user/total?id={id}";
            Loader loader = new Loader();
            var result = await loader.Load(apiUrl);
            return result;
        }

        public static async Task<string> GetAverageTimeForUser(string id)
        {
            string apiUrl = $"http://localhost:5169/api/stats/user/average?id={id}";
            Loader loader = new Loader();
            var result = await loader.Load(apiUrl);
            return result;
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
