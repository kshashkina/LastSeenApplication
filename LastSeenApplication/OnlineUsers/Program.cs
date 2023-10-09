using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;



bool shouldRun = true;
Dictionary<DateTime, int> onlineUsersCount = new Dictionary<DateTime, int>();
while (shouldRun)
{
    int offset = 0;
    int onlineUsers = 0;
    while (true)
    {
        var userData = FetchUserData(offset);
        if (userData == null || userData.Length == 0)
        {
            onlineUsersCount.Add(DateTime.Now, onlineUsers);
            Console.WriteLine($"Online users count: {onlineUsers}");
            string formattedDateTime = DateTime.Now.ToString("yyyy-dd-MM-HH:mm:ss");
            var onlineUser = new OnlineUsersData
            {
                OnlineUsersCount = onlineUsers,
                Timestamp  = formattedDateTime
            };
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
            System.Threading.Thread.Sleep(10000);
            break;
        }
        foreach (var user in userData)
        {
            if (user.lastSeenDate == null)
            {
                onlineUsers++;
            }
        }


        offset += 20; 
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
}
public class UserData
{
    public User[] data { get; set; }
}

public class OnlineUsersData
{
    public string Timestamp { get; set; }
    public int OnlineUsersCount { get; set; }
}