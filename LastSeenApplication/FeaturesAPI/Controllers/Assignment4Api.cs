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

            OnlineUsersData reader = new OnlineUsersData();
            var onlineUsersDataList = reader.ReaderTimeCount(filePathFirst, id);

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
    private readonly string filePathSecond = @"..\averageTime\bin\Debug\net7.0\online.json";

    [HttpGet()]
    public IActionResult GetOnlineUsersData([FromQuery] string id)
    {
        try
        {

            if (!System.IO.File.Exists(filePathSecond))
            {
                return NotFound("JSON file not found");
            }

            OnlineUsersData reader = new OnlineUsersData();
            var onlineUsersDataList = reader.ReaderTimeCount(filePathSecond, id);


            List<int> allTime = new List<int>();
            foreach (var user in onlineUsersDataList)
            {
                allTime.Add(user.wasTimeOnline);
            }
            var averageWeek = (int)Math.Round(allTime.Average());
            var averageDay = (int)(averageWeek / 7);

            return Ok(new { averageWeek, averageDay });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }
}

[Route("api/user")]
[ApiController]
public class UserController : ControllerBase
{
    public List<string> usersToForget = new List<string>();
    
    
    [HttpPost("forget")]
    public IActionResult ForgetUser(string id)
    {
        usersToForget.Add(id);
        string isuseronline = @"..\isUserOnline\bin\Debug\net7.0\online.json";
        string wasonlinetime = @"..\wasOnlineTime\bin\Debug\net7.0\online.json";
        string averagetime = @"..\averageTime\bin\Debug\net7.0\online.json";

        OnlineUsersData remover = new OnlineUsersData();
        remover.Remove(isuseronline,id);
        remover.Remove(wasonlinetime,id);
        remover.Remove(averagetime, id);
        
        return Ok(new { id });
    }
}

