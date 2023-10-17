using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OnlineApi;

[Route("api/stats")]
[ApiController]
public class PersonController : ControllerBase
{
    private readonly string filePathFirst = @"..\OnlineUsers\bin\Debug\net7.0\online.json";

    [HttpGet("users")]
    public IActionResult GetOnlineUsersData([FromQuery] string date)
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

                if (onlineUserData.Timestamp == date)
                {
                    onlineUsersDataList.Add(onlineUserData);
                }
            }

            int? usersOnline = onlineUsersDataList.Count > 0
                ? onlineUsersDataList.Last().OnlineUsersCount
                : (int?)null;

            return Ok(new { usersOnline });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }
    private readonly string filePath = @"..\isUserOnline\bin\Debug\net7.0\online.json";

    [HttpGet("user")]
    public IActionResult GetUserOnlineData([FromQuery] string date, [FromQuery] string userId)
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

                if (onlineUserData.Timestamp == date && onlineUserData.userId == userId)
                {
                    onlineUsersDataList.Add(onlineUserData);
                }
            }
            
            var response = new
            {
                onlineUsersDataList.Last().isOnline,
                onlineUsersDataList.Last().lastSeen
            };
            

            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }
}