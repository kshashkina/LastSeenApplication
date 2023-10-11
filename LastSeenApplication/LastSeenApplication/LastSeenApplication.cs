using System;
using Newtonsoft.Json;

namespace LastSeenApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string language = ChooseLanguage();
            Console.WriteLine("What you want to do? Have a list of all users - 1, have number of users at the exact time - 2");
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

        public static string ChooseLanguage()
        {
            Console.WriteLine("Choose the language:");
            Console.WriteLine("1. English");
            Console.WriteLine("2. Русский");
            Console.WriteLine("3. Українська");
            Console.WriteLine("4. Deutsch");
            Console.WriteLine("5. Français");
            int languageChoice = Convert.ToInt32(Console.ReadLine());

            string language = "en";

            switch (languageChoice)
            {
                case 1:
                    language = "en";
                    break;
                case 2:
                    language = "ru";
                    break;
                case 3:
                    language = "uk";
                    break;
                case 4:
                    language = "de";
                    break;
                case 5:
                    language = "fr";
                    break;

                default:
                    Console.WriteLine("There is no such language, starting in English");
                    break;
            }

            return language;
        }

        public static void RunLastSeenApplication(int startingOffset, string language)
        {
            int offset = startingOffset;

            while (true)
            {
                var userData = FetchUserData(offset);
                if (userData == null || userData.Length == 0)
                {
                    return;
                }

                foreach (var user in userData)
                {
                    var formattedData = FormatUserData(user, language);
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

        public static string FormatUserData(User user, string language)
        {
            string nickName = user.nickname;

            if (user.lastSeenDate == null)
            {
                switch (language)
                {
                    case "en":
                        return $"{nickName} is online.";
                    case "ru":
                        return $"{nickName} в сети.";
                    case "uk":
                        return $"{nickName} у мережі.";
                    case "de":
                        return $"{nickName} ist online.";
                    case "fr":
                        return $"{nickName} est en ligne.";
                    case "zh":
                        return $"{nickName} 在線。";
                    default:
                        return $"{nickName} is online.";
                }
            }
            else
            {
                DateTime now = DateTime.Now;
                DateTime givenDate = user.lastSeenDate.Value;
                TimeSpan difference = now - givenDate;
                string timeAgo = GetTimeAgoString(difference, language);

                switch (language)
                {
                    case "en":
                        return $"{nickName} {timeAgo}";
                    case "ru":
                        return $"{nickName} был(а) в сети {timeAgo}";
                    case "uk":
                        return $"{nickName} був(ла) у мережі {timeAgo}";
                    case "de":
                        return $"{nickName} war vor {timeAgo} online.";
                    case "fr":
                        return $"{nickName} était en ligne il y a {timeAgo}.";
                    default:
                        return $"{nickName} {timeAgo}";
                }
            }
        }

        public static string GetTimeAgoString(TimeSpan difference, string language)
        {
            switch (language)
            {
                case "en":
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
                        return "a couple of minutes ago";
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
                        return "a long time ago";
                    }
                case "ru":
                    if (difference.TotalSeconds <= 30)
                    {
                        return "только что";
                    }
                    else if (difference.TotalSeconds <= 60)
                    {
                        return "меньше минуты назад";
                    }
                    else if (difference.TotalMinutes <= 59)
                    {
                        return "пару минут назад";
                    }
                    else if (difference.TotalMinutes <= 119)
                    {
                        return "час назад";
                    }
                    else if (difference.TotalMinutes <= 23 * 60)
                    {
                        return "сегодня";
                    }
                    else if (difference.TotalMinutes <= 47 * 60)
                    {
                        return "вчера";
                    }
                    else if (difference.TotalDays < 7)
                    {
                        return "на этой неделе";
                    }
                    else
                    {
                        return "давно";
                    }
                case "uk":
                    if (difference.TotalSeconds <= 30)
                    {
                        return "лише що";
                    }
                    else if (difference.TotalSeconds <= 60)
                    {
                        return "менше хвилини тому";
                    }
                    else if (difference.TotalMinutes <= 59)
                    {
                        return "кілька хвилин тому";
                    }
                    else if (difference.TotalMinutes <= 119)
                    {
                        return "годину тому";
                    }
                    else if (difference.TotalMinutes <= 23 * 60)
                    {
                        return "сьогодні";
                    }
                    else if (difference.TotalMinutes <= 47 * 60)
                    {
                        return "вчора";
                    }
                    else if (difference.TotalDays < 7)
                    {
                        return "цього тижня";
                    }
                    else
                    {
                        return "давно";
                    }
                case "de":
                    if (difference.TotalSeconds <= 30)
                    {
                        return "gerade eben";
                    }
                    else if (difference.TotalSeconds <= 60)
                    {
                        return "vor weniger als einer Minute";
                    }
                    else if (difference.TotalMinutes <= 59)
                    {
                        return "vor ein paar Minuten";
                    }
                    else if (difference.TotalMinutes <= 119)
                    {
                        return "vor einer Stunde";
                    }
                    else if (difference.TotalMinutes <= 23 * 60)
                    {
                        return "heute";
                    }
                    else if (difference.TotalMinutes <= 47 * 60)
                    {
                        return "gestern";
                    }
                    else if (difference.TotalDays < 7)
                    {
                        return "diese Woche";
                    }
                    else
                    {
                        return "vor langer Zeit";
                    }
                case "fr":
                    if (difference.TotalSeconds <= 30)
                    {
                        return "à l'instant";
                    }
                    else if (difference.TotalSeconds <= 60)
                    {
                        return "il y a moins d'une minute";
                    }
                    else if (difference.TotalMinutes <= 59)
                    {
                        return "il y a quelques minutes";
                    }
                    else if (difference.TotalMinutes <= 119)
                    {
                        return "il y a une heure";
                    }
                    else if (difference.TotalMinutes <= 23 * 60)
                    {
                        return "aujourd'hui";
                    }
                    else if (difference.TotalMinutes <= 47 * 60)
                    {
                        return "hier";
                    }
                    else if (difference.TotalDays < 7)
                    {
                        return "cette semaine";
                    }
                    else
                    {
                        return "il y a longtemps";
                    }
                default:
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
