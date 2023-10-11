using Newtonsoft.Json;

bool shouldRun = true;
while (shouldRun)
{
    int offset = 0;
    while (true)
    {
        var userData = FetchUserData(offset);
        if (userData == null || userData.Length == 0)
        {
            break;
        }
        foreach (var user in userData)
        {
            var userId = user.UserId;
            var lastSeen = user.LastSeenDate.ToString();
            var isOnline = user.isOnline;
            var onlineUser = new OnlineUsersData
            {
                Timestamp = DateTime.Now.ToString(("yyyy-dd-MM-HH:mm:ss")),
                userId = userId,
                lastSeen = lastSeen,
                isOnline = isOnline
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
        }


        offset += 20; 
    }
    System.Threading.Thread.Sleep(10000);

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
    public DateTime? LastSeenDate { get; set; }
    public string UserId { get; set; }
    public string isOnline { get; set; }

}
public class UserData
{
    public User[] data { get; set; }
}

public class OnlineUsersData
{
    public string lastSeen { get; set; }
    public string userId { get; set; }
    
    public string Timestamp { get; set; }
    public string isOnline { get; set; }

}