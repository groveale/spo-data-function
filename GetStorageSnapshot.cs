using System;
using groveale.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace groveale
{
    public class GetStorageSnapshot
    {
        private readonly ILogger _logger;
        private readonly IStorageSnapshotService _storageSnapshotService;
        private readonly IGraphService _graphService;


        public GetStorageSnapshot(ILoggerFactory loggerFactory, IStorageSnapshotService storageSnapshotService, IGraphService graphService)
        {
            _logger = loggerFactory.CreateLogger<GetStorageSnapshot>();
            _storageSnapshotService = storageSnapshotService;
            _graphService = graphService;
        }

        [Function("GetStorageSnapshot")]
        public async Task Run([TimerTrigger("0 0 3 * * *")] TimerInfo myTimer)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            
            if (myTimer.ScheduleStatus is not null)
            {
                _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
            }

           
            var token = await _graphService.GetAuthToken();
            var spoSitesData = await _graphService.GetSPOSiteReportAsync();
            var tenantSnap = await _graphService.GetTenantStorageReportAsync();

            _logger.LogInformation($"SiteCount: {spoSitesData.Count}");
            _logger.LogInformation($"SiteCount: {tenantSnap}");
        }
    }
}
