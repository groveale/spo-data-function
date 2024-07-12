using System;
using groveale.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace groveale
{
    public class GetM365UsageReport
    {
        private readonly ILogger _logger;
        private readonly IGraphService _graphService;
        private readonly ICSVFileService _csvFileService;

        public GetM365UsageReport(ILoggerFactory loggerFactory, IGraphService graphService, ICSVFileService cSVFileService)
        {
            _logger = loggerFactory.CreateLogger<GetM365UsageReport>();
            _graphService = graphService;
            _csvFileService = cSVFileService;
        }

        [Function("GetM365UsageReport")]
        public async Task Run([TimerTrigger("0 0 3 * * *")] TimerInfo myTimer)
        {
            _logger.LogInformation($"M365 Usage Report Timer trigger function executed at: {DateTime.Now}");
            
            if (myTimer.ScheduleStatus is not null)
            {
                _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
            }

            // Read in the DaysToLookBack from the environment (convert to int)
            var daysToLookBackString = Environment.GetEnvironmentVariable("DaysToLookBack");

            if (int.TryParse(daysToLookBackString, out int daysToLookBack))
            {
                // Conversion successful, use daysToLookBack as an int
            }
            else
            {
                // Conversion failed, use a default value
                daysToLookBack = 7;
            }

            // No report data for today and yesterday, so start from two days ago
            var twoDaysAgo = DateTime.Now.AddDays(-2);

            var driveId = await _graphService.GetDriveIdAsync();

            for (int i = 0; i < daysToLookBack; i++)
            {
                var reportDate = twoDaysAgo.AddDays(-i);
                var usageReports = await _graphService.GetM365AppUsageReportAsync(reportDate);

                _logger.LogInformation($"UsageReports for {reportDate.ToString("yyyy-MM-dd")}: {usageReports.Count}");

                // Generate CSV file
                var csvBytes = await _csvFileService.ConvertM365ReportToCsvAndReturnAsBytesAsync(usageReports);

                // Upload CSV file
                var uploaded = await _graphService.UploadFileToSharePointAsync(csvBytes, driveId, $"M365UsageReport_{reportDate.ToString("yyyy-MM-dd")}.csv");

                if (uploaded)
                {
                    _logger.LogInformation($"M365 Usage Report for {reportDate.ToString("yyyy-MM-dd")} uploaded successfully");
                }
                else
                {
                    _logger.LogError($"M365 Usage Report for {reportDate.ToString("yyyy-MM-dd")} failed to upload");
                }
            }

        }
    }
}
