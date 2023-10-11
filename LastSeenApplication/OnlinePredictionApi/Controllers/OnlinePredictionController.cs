using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OnlinePredictionApi;

[Route("api/prediction")]
[ApiController]
public class PersonController : ControllerBase
{
    private readonly string filePath = @"..\OnlineUsers\bin\Debug\net7.0\online.json";

    [HttpGet("user")]
    public IActionResult GetUserOnlineData([FromQuery] string date)
    {
        try
        {
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("JSON file not found");
            }

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

            var average = (int)Math.Round(onlineUsersDataList.Average());


            int? usersOnline = onlineUsersDataList.Count > 0 ? average : (int?)null;
            

            return Ok(new { usersOnline });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }
}