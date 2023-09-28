using Newtonsoft.Json;

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
                        
                    }
                    else
                    {
                        Console.WriteLine("Error: " + response.StatusCode);
                    }
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
