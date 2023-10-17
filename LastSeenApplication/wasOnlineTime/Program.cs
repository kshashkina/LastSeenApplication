using Newtonsoft.Json;

bool shouldRun = true;
Dictionary<string, int> onlineUsersCount = new Dictionary<string, int>();

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

            foreach (var user in onlineUsersCount)
            {
                var onlineUser = new OnlineUsersData
                {
                    id  = user.Key,
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
            if (user.lastSeenDate == null)
            {
                if (onlineUsersCount.ContainsKey(user.userId))
                {
                    onlineUsersCount[user.userId]++;

                }
                else
                {
                    onlineUsersCount[user.userId] = 1;
                }
            }
        }

        offset += userData.Length;
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
    public string userId { get; set; }
}
public class UserData
{
    public User[] data { get; set; }
}

public class OnlineUsersData
{
    public string id { get; set; }
    public int wasTimeOnline { get; set; }
}