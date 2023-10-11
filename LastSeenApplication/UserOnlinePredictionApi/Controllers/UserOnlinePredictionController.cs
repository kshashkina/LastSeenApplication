using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UserOnlinePredictionApi;

[Route("api/prediction")]
[ApiController]
public class PersonController : ControllerBase
{
    private readonly string filePath = @"..\isUserOnline\bin\Debug\net7.0\online.json";

    [HttpGet("user")]
    public IActionResult GetUserOnlineData([FromQuery] string date, [FromQuery] double tolerance, [FromQuery] string userId)
    {
        try
        {
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("JSON file not found");
            }

            List<OnlineUsersData> onlineUsersDataList = new List<OnlineUsersData>();
            
            

            foreach (string line in System.IO.File.ReadLines(filePath))
            {
                var onlineUserData = JsonConvert.DeserializeObject<OnlineUsersData>(line);
                DateTime inputDateTime = DateTime.ParseExact(onlineUserData.Timestamp, "yyyy-MM-dd-HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);  
                DateTime resultDateTime = inputDateTime.AddDays(7);
                string resultString = resultDateTime.ToString("yyyy-MM-dd-HH:mm:ss");

                if (resultString == date && onlineUserData.userId == userId )
                {
                    onlineUsersDataList.Add(onlineUserData);
                }
                resultDateTime = inputDateTime.AddDays(7);
                resultString = resultDateTime.ToString("yyyy-MM-dd-HH:mm:ss");
                if (resultString == date && onlineUserData.userId == userId)
                {
                    onlineUsersDataList.Add(onlineUserData);
                }
                resultDateTime = inputDateTime.AddDays(7);
                resultString = resultDateTime.ToString("yyyy-MM-dd-HH:mm:ss");
                if (resultString == date && onlineUserData.userId == userId)
                {
                    onlineUsersDataList.Add(onlineUserData);
                }
            }

            var chance = 0;
            bool isOnline = false;

            foreach (var line in onlineUsersDataList)
            {
                if (line.isOnline)
                {
                    chance++;
                }
            }

            double chancePercent = chance / onlineUsersDataList.Count;
            if (chancePercent > tolerance)
            {
                isOnline = true;
            }
            
            
            
            var response = new
            {
                isOnline,
                chancePercent
            };
            
            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }
}