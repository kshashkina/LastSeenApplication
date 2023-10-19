using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OnlineApi;

[Route("api/stats/user")]
[ApiController]
public class TotalTimeOnline : ControllerBase
{
    private readonly string filePathFirst = @"..\wasOnlineTime\bin\Debug\net7.0\online.json";

    [HttpGet("total")]
    public IActionResult GetOnlineUsersData([FromQuery] string id)
    {
        try
        {

            if (!System.IO.File.Exists(filePathFirst))
            {
                return NotFound("JSON file not found");
            }

            List<OnlineUsersData> onlineUsersDataList = new List<OnlineUsersData>();

            foreach (string line in System.IO.File.ReadLines(filePathFirst))
            {
                var onlineUserData = JsonConvert.DeserializeObject<OnlineUsersData>(line);

                if (onlineUserData.userId == id)
                {
                    onlineUsersDataList.Add(onlineUserData);
                }
            }

            int? timeOnline = onlineUsersDataList.Count > 0
                ? onlineUsersDataList.Last().wasTimeOnline
                : (int?)null;

            return Ok(new { usersOnline = timeOnline });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }
}

[Route("api/stats/user/average")]
[ApiController]
public class AverageTimeOnline : ControllerBase
{
    private readonly string filePathFirst = @"..\averageTime\bin\Debug\net7.0\online.json";

    [HttpGet()]
    public IActionResult GetOnlineUsersData([FromQuery] string id)
    {
        try
        {

            if (!System.IO.File.Exists(filePathFirst))
            {
                return NotFound("JSON file not found");
            }

            List<int> onlineUsersDataList = new List<int>();

            foreach (string line in System.IO.File.ReadLines(filePathFirst))
            {
                var onlineUserData = JsonConvert.DeserializeObject<OnlineUsersData>(line);

                if (onlineUserData.userId == id)
                {
                    onlineUsersDataList.Add(onlineUserData.wasTimeOnline);
                }
            }

            var averageWeek = (int)Math.Round(onlineUsersDataList.Average());
            var averageDay = (int)(averageWeek / 7);

            return Ok(new { averageWeek, averageDay });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }
}