using Azure.Identity;
using Azure.Core;
using Microsoft.Graph.Beta;
using groveale.Models;
using System.Text.Json;
using DriveUpload = Microsoft.Graph.Beta.Drives.Item.Items.Item.CreateUploadSession;
using Microsoft.Graph.Beta.Models;
using Microsoft.Graph.Beta.Models.ODataErrors;

namespace groveale.Services 
{
    public interface IGraphService
    {
        Task<string> GetAuthToken();
        Task<List<SiteReport>> GetSPOSiteReportAsync();
        // Add other methods as needed
        Task<TenantStorageReport> GetTenantStorageReportAsync();

        Task<List<M365AppUsageReport>> GetM365AppUsageReportAsync(DateTime reportDate);

        Task<bool> UploadFileToSharePointAsync(byte[] fileContent, string driveId, string fileName);

        Task<string> GetDriveIdAsync();
    }

    public class GraphService : IGraphService
    {
        private GraphServiceClient _graphServiceClient;
        private DefaultAzureCredential _defaultCredential;

        public GraphService()
        {
            _defaultCredential = new DefaultAzureCredential();
            _graphServiceClient = new GraphServiceClient(_defaultCredential,
                // Use the default scope, which will request the scopes
                // configured on the app registration
                new[] {"https://graph.microsoft.com/.default"});
        }

        public async Task<string> GetAuthToken()
        {
            // Request token with given scopes
            var context = new TokenRequestContext(new[] {"https://graph.microsoft.com/.default"});
            var response = await _defaultCredential.GetTokenAsync(context);
            return response.Token;
        }

        public async Task<List<M365AppUsageReport>> GetM365AppUsageReportAsync(DateTime reportDate)
        {
            // Date in string format YYYY-MM-DD
            //string reportDateString = reportDate.Value.ToString("yyyy-MM-dd");
            Microsoft.Kiota.Abstractions.Date date = new Microsoft.Kiota.Abstractions.Date(reportDate);

            var urlString = _graphServiceClient.Reports.GetM365AppUserDetailWithDate(date).ToGetRequestInformation().URI.OriginalString;
            urlString += "?$format=application/json";//append the query parameter
            var m365AppUsageReportResponse = await _graphServiceClient.Reports.GetM365AppUserDetailWithDate(date).WithUrl(urlString).GetAsync();

            byte[] buffer = new byte[8192];
            int bytesRead;
            List<M365AppUsageReport> m365AppUsageReports = new List<M365AppUsageReport>();

            do {

                string usageReportsInChunk = "";

                while ((bytesRead = await m365AppUsageReportResponse.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    // Process the chunk of data here
                    string chunk = System.Text.Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    usageReportsInChunk += chunk;
                }

                using (JsonDocument doc = JsonDocument.Parse(usageReportsInChunk))
                {
                    // Append the site data to the site dataString
                    if (doc.RootElement.TryGetProperty("value", out JsonElement usageReports))
                    {
                        var reports = JsonSerializer.Deserialize<List<M365AppUsageReport>>(usageReports.GetRawText());
                        m365AppUsageReports.AddRange(reports);
                    }

                    if (doc.RootElement.TryGetProperty("@odata.nextLink", out JsonElement nextLinkElement))
                    {
                        urlString = nextLinkElement.GetString(); 
                    }
                    else
                    {
                        urlString = null; // No more pages break out of the loop
                        break;
                    }
                }

                m365AppUsageReportResponse = await _graphServiceClient.Reports.GetM365AppUserDetailWithDate(date).WithUrl(urlString).GetAsync();

            } while (urlString != null);


            return m365AppUsageReports;
        }

        public async Task<List<SiteReport>> GetSPOSiteReportAsync()
        {
            // We have to do some interesting things here to get the data in JSON from this graph endpoint
            var urlString = _graphServiceClient.Reports.GetSharePointSiteUsageDetailWithPeriod("D30").ToGetRequestInformation().URI.OriginalString;
            urlString += "?$format=application/json";//append the query parameter
            var spoSitesDataJSON = await _graphServiceClient.Reports.GetSharePointSiteUsageDetailWithPeriod("D30").WithUrl(urlString).GetAsync();

            byte[] buffer = new byte[8192];
            int bytesRead;
            List<SiteReport> spoSitesData = new List<SiteReport>();

            do {

                string sitesInChunk = "";

                while ((bytesRead = await spoSitesDataJSON.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    // Process the chunk of data here
                    string chunk = System.Text.Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    sitesInChunk += chunk;
                }

                using (JsonDocument doc = JsonDocument.Parse(sitesInChunk))
                {
                    // Append the site data to the site dataString
                    if (doc.RootElement.TryGetProperty("value", out JsonElement sites))
                    {
                        spoSitesData.AddRange(JsonSerializer.Deserialize<List<SiteReport>>(sites));
                    }

                    if (doc.RootElement.TryGetProperty("@odata.nextLink", out JsonElement nextLinkElement))
                    {
                        urlString = nextLinkElement.GetString(); 
                    }
                    else
                    {
                        urlString = null; // No more pages break out of the loop
                        break;
                    }
                }

                spoSitesDataJSON = await _graphServiceClient.Reports.GetSharePointSiteUsageDetailWithPeriod("D30").WithUrl(urlString).GetAsync();

            } while (urlString != null);

            return spoSitesData;
        }

        public async Task<TenantStorageReport> GetTenantStorageReportAsync()
        {
            var urlString = _graphServiceClient.Reports.GetSharePointSiteUsageStorageWithPeriod("D7").ToGetRequestInformation().URI.OriginalString;
            urlString += "?$format=application/json";//append the query parameter
            var spoTenantSnapshot = await _graphServiceClient.Reports.GetSharePointSiteUsageStorageWithPeriod("D7").WithUrl(urlString).GetAsync();

            byte[] buffer = new byte[8192];
            int bytesRead;
            List<TenantStorageReport> spoTenantStorage = new List<TenantStorageReport>();
            string snapshotsInChunk = "";

            while ((bytesRead = await spoTenantSnapshot.ReadAsync(buffer, 0, buffer.Length)) > 0)
            {
                // Process the chunk of data here
                string chunk = System.Text.Encoding.UTF8.GetString(buffer, 0, bytesRead);
                snapshotsInChunk += chunk;
            }

            using (JsonDocument doc = JsonDocument.Parse(snapshotsInChunk))
            {
                // Append the site data to the site dataString
                if (doc.RootElement.TryGetProperty("value", out JsonElement tenantSnaps))
                {
                    spoTenantStorage.AddRange(JsonSerializer.Deserialize<List<TenantStorageReport>>(tenantSnaps));
                }
            }

            // we are only returning the lastest snapshot
            return spoTenantStorage.FirstOrDefault();

        }

        public async Task<string> GetDriveIdAsync()
        {
            var siteId = System.Environment.GetEnvironmentVariable("M365UsageDataSiteId");
            var libraryName = System.Environment.GetEnvironmentVariable("AppUsageProcessedLibraryName");

            var drives = await _graphServiceClient.Sites[siteId].Drives.GetAsync();

            // filter the drives to get the one we want
            var drive = drives.Value.FirstOrDefault(d => d.Name == libraryName);

            if (drive == null)
            {
                throw new Exception($"Drive with name {libraryName} not found in site {siteId}");
            }

            return drive.Id;
        }

        public async Task<bool> UploadFileToSharePointAsync(byte[] fileContent, string driveId, string fileName)
        {
            // Look here: https://learn.microsoft.com/en-us/graph/sdks/large-file-upload?tabs=csharp

            // Use properties to specify the conflict behavior
            //using DriveUpload = Microsoft.Graph.Drives.Item.Items.Item.CreateUploadSession;
            var uploadSessionRequestBody = new DriveUpload.CreateUploadSessionPostRequestBody
            {
                Item = new DriveItemUploadableProperties
                {
                    AdditionalData = new Dictionary<string, object>
                    {
                        { "@microsoft.graph.conflictBehavior", "replace" },
                    },
                },
            };

            // Create the upload session
            // itemPath does not need to be a path to an existing item
            var drive = await _graphServiceClient.Drives[driveId].GetAsync();
            var uploadSession = await _graphServiceClient.Drives[driveId]
                .Items["root"]
                .ItemWithPath(fileName)
                .CreateUploadSession
                .PostAsync(uploadSessionRequestBody);

            // Create a stream from a byte array
            using var fileStream = new MemoryStream(fileContent);

            // Max slice size must be a multiple of 320 KiB
            int maxSliceSize = 320 * 1024;
            var fileUploadTask = new Microsoft.Graph.LargeFileUploadTask<DriveItem>(
                uploadSession, fileStream, maxSliceSize, _graphServiceClient.RequestAdapter);

            var totalLength = fileStream.Length;
            // Create a callback that is invoked after each slice is uploaded
            IProgress<long> progress = new Progress<long>(prog =>
            {
                Console.WriteLine($"Uploaded {prog} bytes of {totalLength} bytes");
            });

            try
            {
                // Upload the file
                var uploadResult = await fileUploadTask.UploadAsync(progress);

                Console.WriteLine(uploadResult.UploadSucceeded ?
                    $"Upload complete, item ID: {uploadResult.ItemResponse.Id}" :
                    "Upload failed");
            }
            catch (ODataError ex)
            {
                Console.WriteLine($"Error uploading: {ex.Error?.Message}");
                return false;
            }

            return true;
        }
    }
}