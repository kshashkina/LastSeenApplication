using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OnlineApi;

[Route("api/prediction")]
[ApiController]
public class PersonPrediction : ControllerBase
{
    private readonly string filePathFirst = @"..\OnlineUsers\bin\Debug\net7.0\online.json";

    [HttpGet("user/average")]
    public IActionResult GetUserOnlineData([FromQuery] string date)
    {
        try
        {
            if (!System.IO.File.Exists(filePathFirst))
            {
                return NotFound("JSON file not found");
            }
            
            OnlineUsersData reader = new OnlineUsersData();
            var onlineUsersDataList = reader.ReaderOnlineCountPreddiction(filePathFirst, date);
            var average = (int)Math.Round(onlineUsersDataList.Average());
            int? usersOnline = onlineUsersDataList.Count > 0 ? average : (int?)null;

            return Ok(new { usersOnline });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }

    private readonly string filePathSecond = @"..\isUserOnline\bin\Debug\net7.0\online.json";

    [HttpGet("user/status")]
    public IActionResult GetUserOnlineStatus([FromQuery] string date, [FromQuery] double tolerance, [FromQuery] string id)
    {
        try
        {
            if (!System.IO.File.Exists(filePathSecond))
            {
                return NotFound("JSON file not found");
            }
            
            OnlineUsersData reader = new OnlineUsersData();
            var onlineUsersDataList = reader.ReaderUserPrediction(filePathSecond, date, id);
            
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
