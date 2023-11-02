using Newtonsoft.Json;

namespace OnlineApi;

public class Final
{
    public string lastSeen { get; set; }
    public string userId { get; set; }
    public string nickname { get; set; }

    
}

public class OnlineUsersData
{
    public string lastSeen { get; set; }
    public string userId { get; set; }
    public int OnlineUsersCount { get; set; }
    
    public string Timestamp { get; set; }
    
    public bool isOnline { get; set; }
    
    public int wasTimeOnline { get; set; }
    
    public string nickname { get; set; }

    public void Remove(string fileName, string id)
    {
        List<string> newLines = new List<string>();
        foreach (string line in System.IO.File.ReadLines(fileName))
        {
            var onlineUserData = JsonConvert.DeserializeObject<OnlineUsersData>(line);

            if (onlineUserData.userId != id)
            {
                newLines.Add(line);
            }
        }
        System.IO.File.WriteAllLines(fileName, newLines);
        newLines.Clear();
    }

    public List<OnlineUsersData> ReaderTimeCount(string filepath, string id)
    {
        List<OnlineUsersData> onlineUsersDataList = new List<OnlineUsersData>();

        foreach (string line in System.IO.File.ReadLines(filepath))
        {
            var onlineUserData = JsonConvert.DeserializeObject<OnlineUsersData>(line);

            if (onlineUserData.userId == id)
            {
                onlineUsersDataList.Add(onlineUserData);
            }
        }

        return onlineUsersDataList;
    }

    public List<int> ReaderOnlineCountPreddiction(string filePath, string date)
    {
        List<int> onlineUsersDataList = new List<int>();

        foreach (string line in System.IO.File.ReadLines(filePath))
        {
            var onlineUserData = JsonConvert.DeserializeObject<OnlineUsersData>(line);
            DateTime inputDateTime = DateTime.ParseExact(onlineUserData.Timestamp, "yyyy-MM-dd-HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
            DateTime resultDateTime = inputDateTime.AddDays(7);
            string resultString = resultDateTime.ToString("yyyy-MM-dd-HH:mm:ss");

            if (resultString == date)
            {
                onlineUsersDataList.Add(onlineUserData.OnlineUsersCount);
            }
            resultDateTime = inputDateTime.AddDays(7);
            resultString = resultDateTime.ToString("yyyy-MM-dd-HH:mm:ss");
            if (resultString == date)
            {
                onlineUsersDataList.Add(onlineUserData.OnlineUsersCount);
            }
            resultDateTime = inputDateTime.AddDays(7);
            resultString = resultDateTime.ToString("yyyy-MM-dd-HH:mm:ss");
            if (resultString == date)
            {
                onlineUsersDataList.Add(onlineUserData.OnlineUsersCount);
            }
        }
        return onlineUsersDataList;
    }

    public List<OnlineUsersData> ReaderUserPrediction(string filePath, string date, string id)
    {
        List<OnlineUsersData> onlineUsersDataList = new List<OnlineUsersData>();

        foreach (string line in System.IO.File.ReadLines(filePath))
        {
            var onlineUserData = JsonConvert.DeserializeObject<OnlineUsersData>(line);
            DateTime inputDateTime = DateTime.ParseExact(onlineUserData.Timestamp, "yyyy-MM-dd-HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
            DateTime resultDateTime = inputDateTime.AddDays(7);
            string resultString = resultDateTime.ToString("yyyy-MM-dd-HH:mm:ss");

            if (resultString == date && onlineUserData.userId == id)
            {
                onlineUsersDataList.Add(onlineUserData);
            }
            resultDateTime = inputDateTime.AddDays(7);
            resultString = resultDateTime.ToString("yyyy-MM-dd-HH:mm:ss");
            if (resultString == date && onlineUserData.userId == id)
            {
                onlineUsersDataList.Add(onlineUserData);
            }
            resultDateTime = inputDateTime.AddDays(7);
            resultString = resultDateTime.ToString("yyyy-MM-dd-HH:mm:ss");
            if (resultString == date && onlineUserData.userId == id)
            {
                onlineUsersDataList.Add(onlineUserData);
            }
        }

        return onlineUsersDataList;
    }

    public List<OnlineUsersData> ReaderOnlineCount(string filePath, string date)
    {
        List<OnlineUsersData> onlineUsersDataList = new List<OnlineUsersData>();

        foreach (string line in System.IO.File.ReadLines(filePath))
        {
            var onlineUserData = JsonConvert.DeserializeObject<OnlineUsersData>(line);

            if (onlineUserData.Timestamp == date)
            {
                onlineUsersDataList.Add(onlineUserData);
            }
        }

        return onlineUsersDataList;
    }

    public List<OnlineUsersData> ReaderisUserOnline(string filePath, string date, string id)
    {
        List<OnlineUsersData> onlineUsersDataList = new List<OnlineUsersData>();

        foreach (string line in System.IO.File.ReadLines(filePath))
        {
            var onlineUserData = JsonConvert.DeserializeObject<OnlineUsersData>(line);

            if (onlineUserData.Timestamp == date && onlineUserData.userId == id)
            {
                onlineUsersDataList.Add(onlineUserData);
            }
        }

        return onlineUsersDataList;
    }
}


public class CreateReportRequest
{
    public ReportMetrics Metrics { get; set; }
    public string Users { get; set; }
}

public class ReportMetrics
{
    public int? DailyAverage { get; set; }
    public int? WeeklyAverage { get; set; }
    public int? Total { get; set; }
    public int? Min { get; set; }
    public int? Max { get; set; }
}

public class Report
{
    public Dictionary<string, ReportMetrics> report { get; set; }
}
