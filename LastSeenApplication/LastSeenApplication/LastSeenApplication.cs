using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LastSeenApplication
{
    public class Program
    {
        public static async Task Main(string[] args)
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
                    await GetOnlineUsersCount(date);
                    break;
                case 3:
                    var (dateUser, id) = localization.SecondFeatureTranslation(language);
                    await GetUserDate(dateUser, id);
                    break;
                case 4:
                    var datePrediction = localization.FirstFeatureTranslation(language);
                    await GetPredictionOnline(datePrediction);
                    break;
                case 5:
                    var (datePredictionUser, idPredictionUser, tolerance) = localization.ForthFeatureTranslation(language);
                    await GetPredictionOnlineUser(datePredictionUser, tolerance, idPredictionUser);
                    break;
                case 6:
                    var idTotal = localization.Assignment4Translation(language);
                    await GetTotalTimeForUser(idTotal);
                    break;
                case 7:
                    var idAverage = localization.Assignment4Translation(language);
                    await GetAverageTimeForUser(idAverage);
                    break;
                case 8:
                    var idDelete = localization.Assignment4Translation(language);
                    await DeleteUser(idDelete);
                    break;
                case 9:
                    var (reportNamePost, users, metrics) = localization.FifthFeatureTranslationPost(language);
                    await PostReport(reportNamePost, users, metrics);
                    break;
                case 10:
                    var (reportNameGet, from, to) = localization.FifthFeatureTranslationGet(language);
                    await GetReport(reportNameGet, from, to);
                    break;
                case 11:
                    await GetAllUsers();
                    break;
            }
        }

        public static async Task<string> GetOnlineUsersCount(string date)
        {
            string apiUrl = $"http://localhost:5169/api/stats/users?date={date}";
            return await LoadData(apiUrl);
        }

        public static async Task<string> GetUserDate(string date, string id)
        {
            string apiUrl = $"http://localhost:5169/api/stats/user?date={date}&userId={id}";
            return await LoadData(apiUrl);
        }

        public static async Task<string> GetPredictionOnline(string date)
        {
            string apiUrl = $"http://localhost:5169/api/prediction/user/average?date={date}";
            return await LoadData(apiUrl);
        }

        public static async Task<string> GetPredictionOnlineUser(string date, string tolerance, string id)
        {
            string apiUrl = $"http://localhost:5169/api/prediction/user/status?date={date}&tolerance={tolerance}&userId={id}";
            return await LoadData(apiUrl);
        }

        public static async Task<string> GetTotalTimeForUser(string id)
        {
            string apiUrl = $"http://localhost:5169/api/stats/user/total?id={id}";
            return await LoadData(apiUrl);
        }

        public static async Task<string> GetAverageTimeForUser(string id)
        {
            string apiUrl = $"http://localhost:5169/api/stats/user/average?id={id}";
            return await LoadData(apiUrl);
        }
        
        public static async Task<string> DeleteUser(string id)
        {
            string apiUrl = $"http://localhost:5169/api/user/forget?id={id}";
            return await PostData(apiUrl, id);
        }

        public static async Task<string> PostReport(string reportName, string users, string metrics)
        {
            string apiUrl = $"http://localhost:5169/api/report/{reportName}?users={users}&metrics={metrics}";
            return await PostData(apiUrl, "{}");
        }

        public static async Task<string> GetReport(string reportName, string from, string to)
        {
            string apiUrl = $"http://localhost:5169/api/report/{reportName}?from={from}&to={to}";
            return await LoadData(apiUrl);
        }

        public static async Task<string> GetAllUsers()
        {
            var apiUrl = "http://localhost:5169/api/users/list";
            return await LoadData(apiUrl);
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

        public static async Task<string> LoadData(string apiUrl)
        {
            Loader loader = new Loader();
            var result = await loader.Load(apiUrl);
            return result;
        }

        public static async Task<string> PostData(string apiUrl, string data)
        {
            Loader loader = new Loader();
            var result = await loader.Post(apiUrl, data);
            return result;
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