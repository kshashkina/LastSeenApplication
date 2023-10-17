namespace OnlineApi;


public class OnlineUsersData
{
    public string lastSeen { get; set; }
    public string userId { get; set; }
    public int OnlineUsersCount { get; set; }

    
    public string Timestamp { get; set; }
    
    public bool isOnline { get; set; }
    
    public int wasTimeOnline { get; set; }

}