using Microsoft.AspNetCore.Mvc;

[Route("api/reports")]
[ApiController]
public class Output : ControllerBase
{
    [HttpGet]
    public IActionResult GetReports()
    {
        try
        {
            string directoryPath = @"../FeaturesAPI";

            if (!Directory.Exists(directoryPath))
            {
                return NotFound("Directory was not found");
            }

            string[] allReportFiles = Directory.GetFiles(directoryPath, "*.json");

            string[] excludedFileNames = new[] { "appsettings.json", "appsettings.Development.json" };

            List<ReportOutput> reports = new List<ReportOutput>();

            foreach (var reportFile in allReportFiles)
            {
                if (excludedFileNames.Contains(Path.GetFileName(reportFile)))
                {
                    continue;
                }

                string[] lines = System.IO.File.ReadAllLines(reportFile);

                if (lines.Length > 0)
                {
                    string lastLine = lines.Last(); // Get the last line

                    ReportOutput reportOutput = new ReportOutput
                    {
                        Name = Path.GetFileName(reportFile), // Remove full path
                        _report = lastLine // Store the last line
                    };
                    reports.Add(reportOutput);
                }
            }

            return Ok(reports);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }
}

public class ReportOutput
{
    public string Name { get; set; }
    public string _report { get; set; }
}