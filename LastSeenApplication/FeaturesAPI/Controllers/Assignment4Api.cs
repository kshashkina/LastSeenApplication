using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OnlineApi;

[Route("api/stats/user/total")]
[ApiController]
public class TotalTimeOnline : ControllerBase
{
    private readonly string filePathFirst = @"..\wasOnlineTime\bin\Debug\net7.0\online.json";

    [HttpGet("users")]
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