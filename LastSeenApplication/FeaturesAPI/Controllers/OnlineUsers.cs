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

            OnlineUsersData reader = new OnlineUsersData();
            var onlineUsersDataList = reader.ReaderOnlineCount(filePathFirst, date);

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
    private readonly string filePathSecond = @"..\isUserOnline\bin\Debug\net7.0\online.json";

    [HttpGet("user")]
    public IActionResult GetUserOnlineData([FromQuery] string date, [FromQuery] string userId)
    {
        try
        {
            if (!System.IO.File.Exists(filePathSecond))
            {
                return NotFound("JSON file not found");
            }

            OnlineUsersData reader = new OnlineUsersData();
            var onlineUsersDataList = reader.ReaderisUserOnline(filePathSecond, date, userId);

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
