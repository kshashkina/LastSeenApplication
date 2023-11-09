using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OnlineApi;

[Route("api/report")]
[ApiController]
public class ReportController : ControllerBase
{
    private readonly Dictionary<string, ReportMetrics> reports = new Dictionary<string, ReportMetrics>();

    [HttpPost("{reportName}")]
    public IActionResult CreateReport(string reportName, [FromQuery] string users, string metrics)
    {
        var usersArray = users.Split(',');
        var metricsArray = metrics.Split(',');
        foreach (var user in usersArray)
        {
            reports[user] = GenerateReports(user, metricsArray);
        }

        string reportTime = DateTime.Today.ToString("yyyy-MM-dd");

        string fileName = $"{reportName}-{reportTime}.json";
        using (StreamWriter writer = new StreamWriter(fileName))
        {
            foreach (var line in reports)
            {
                CreateReportRequest reportRequest = new CreateReportRequest
                {
                    Users = line.Key,
                    Metrics = line.Value,
                };
                string json = JsonConvert.SerializeObject(reportRequest);
                writer.WriteLine(json);

            }
        }
        return Ok(new { });
    }
    [HttpGet("{reportName}")]
    public IActionResult GetReport(string reportName, [FromQuery] string from, [FromQuery] string to)
    {
        try
        {
            DateTime startDate = DateTime.ParseExact(from, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            DateTime endDate = DateTime.ParseExact(to, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            List<DateTime> allDates = GetAllDates(startDate, endDate);
            List<string> dates = new List<string>();

            foreach (var date in allDates)
            {
                string formattedDate = date.ToString("yyyy-MM-dd");
                dates.Add(formattedDate);
            }

            Report report = new Report()
            {
                report = new Dictionary<string, ReportMetrics>()
            };

            foreach (var date in dates)
            {
                string filename = $"{reportName}-{date}.json";
                if (!System.IO.File.Exists(filename))
                {
                    return NotFound("JSON file not found");
                }
                foreach (var line in System.IO.File.ReadLines(filename))
                {
                    var onlineUserData = JsonConvert.DeserializeObject<CreateReportRequest>(line);
                    if (onlineUserData != null)
                    {
                        if (onlineUserData.Users != null && onlineUserData.Metrics != null)
                        {
                            if (!report.report.ContainsKey(onlineUserData.Users))
                            {
                                report.report[onlineUserData.Users] = onlineUserData.Metrics;
                            }

                            else if (report.report[onlineUserData.Users] != null)
                            {
                                {
                                    ReportMetrics reportMetrics = report.report[onlineUserData.Users];
                                    reportMetrics.Total += onlineUserData.Metrics.Total;
                                    reportMetrics.WeeklyAverage =
                                        (reportMetrics.WeeklyAverage + onlineUserData.Metrics.WeeklyAverage) / 2;
                                    reportMetrics.DailyAverage =
                                        (reportMetrics.DailyAverage + onlineUserData.Metrics.DailyAverage) / 2;
                                    if (reportMetrics.Min < onlineUserData.Metrics.Min)
                                    {
                                        reportMetrics.Min = onlineUserData.Metrics.Min;
                                    }

                                    if (reportMetrics.Max > onlineUserData.Metrics.Max)
                                    {
                                        reportMetrics.Max = onlineUserData.Metrics.Max;
                                    }
                                }
                            }
                        }
                    }

                }

            }
            return Ok(new { report.report });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }

    private ReportMetrics GenerateReports(string user, string[] metrics)
    {
        string wasonlinetime = @"..\wasOnlineTime\bin\Debug\net7.0\online.json";
        string averagetime = @"..\averageTime\bin\Debug\net7.0\online.json";

        OnlineUsersData reader = new OnlineUsersData();
        var onlineUsersDataListAverage = reader.ReaderTimeCount(averagetime, user);
        List<int> allTime = new List<int>();
        foreach (var id in onlineUsersDataListAverage)
        {
            allTime.Add(id.wasTimeOnline);
        }

        int? averageWeek = null;
        int? averageDay = null;
        int? timeOnline = null;
        int? min = null;
        int? max = null;
        if (metrics.Contains("averageWeek"))
        {
            averageWeek = (int)Math.Round(allTime.Average());
        }

        if (metrics.Contains("averageDay"))
        {
            averageDay = (int?)(averageWeek / 7);
        }

        var onlineUsersDataListTotal = reader.ReaderTimeCount(wasonlinetime, user);
        int sum = 0;
        List<int> MaxMin = new List<int>();
        foreach (var input in onlineUsersDataListTotal)
        {
            sum += input.wasTimeOnline;
            MaxMin.Add(input.wasTimeOnline);
        }

        if (metrics.Contains("Total"))
        {
            timeOnline = onlineUsersDataListTotal.Count > 0
                ? sum
                : null;
        }

        if (metrics.Contains("Max"))
        {
            max = MaxMin.Max();
        }

        if (metrics.Contains("Min"))
        {
            min = MaxMin.Min();
        }

        var report = new ReportMetrics
        {
            DailyAverage = averageDay,
            WeeklyAverage = averageWeek,
            Total = timeOnline,
            Min = min,
            Max = max
        };

        return report;
    }
    private static List<DateTime> GetAllDates(DateTime startDate, DateTime endDate)
    {
        List<DateTime> allDates = new List<DateTime>();

        DateTime currentDate = startDate;

        while (currentDate <= endDate)
        {
            allDates.Add(currentDate);
            currentDate = currentDate.AddDays(1);
        }

        return allDates;
    }
}

