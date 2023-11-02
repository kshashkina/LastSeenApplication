using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OnlineApi;
using System;
using System.Collections.Generic;
using System.IO;

[Route("api")]
[ApiController]
public class PersonControllers : ControllerBase
{
    private readonly string filePath = @"..\allusersdata\bin\Debug\net7.0\online.json";

    [HttpGet("users/list")]
    public IActionResult GetUserOnlineData()
    {
        try
        {
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("JSON file not found");
            }

            List<Final> list = new List<Final>();
            foreach (string line in System.IO.File.ReadLines(filePath))
            {
                var onlineUserData = JsonConvert.DeserializeObject<Final>(line);
                list.Add(onlineUserData);
            }

            return Ok(list); // Return the entire list as the response
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }
}
