using Azure.Identity;
using Azure.Core;
using Microsoft.Graph.Beta;
using groveale.Models;
using System.Text.Json;

namespace groveale.Services 
{
    public interface IGraphService
    {
        Task<string> GetAuthToken();
        Task<List<SiteReport>> GetSPOSiteReportAsync();
        // Add other methods as needed
        Task<TenantStorageReport> GetTenantStorageReportAsync();
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


    }
}