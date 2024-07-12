using System.Text.Json;
using Azure;
using Azure.Data.Tables;
using groveale.Models;

namespace groveale.Services 
{
    public interface IStorageSnapshotService
    {
        Task ProcessTenantSnapshot(TenantStorageReport tenantSnap);
    }

    public class StorageSnapshotService : IStorageSnapshotService
    {
        private readonly TableServiceClient _serviceClient;
        private readonly string _tenantSnapTableName = "TenantStorageSnapshots";
        private readonly string _siteSnapTableName = "SiteStorageSnapshots";
        private readonly string _tenantId;
        public StorageSnapshotService()
        {
            var storageUri = System.Environment.GetEnvironmentVariable("StorageAccountUri");
            var accountName = System.Environment.GetEnvironmentVariable("StorageAccountName");
            var storageAccountKey = System.Environment.GetEnvironmentVariable("StorageAccountKey");
            _tenantId = System.Environment.GetEnvironmentVariable("TenantId");

            _serviceClient = new TableServiceClient(
                new Uri(storageUri),
                new TableSharedKeyCredential(accountName, storageAccountKey));
        }

        public async Task ProcessTenantSnapshot(TenantStorageReport tenantSnap)
        {
            var tableClient = _serviceClient.GetTableClient(_tenantSnapTableName);
            tableClient.CreateIfNotExists();

            // Ensure the ReportDate is specified as UTC
            DateTime reportDateUtc = DateTime.SpecifyKind(tenantSnap.ReportDate, DateTimeKind.Utc);

            // Ensure the ReportRefreshDate is specified as UTC
            DateTime reportRefreshDateUtc = DateTime.SpecifyKind(tenantSnap.ReportRefreshDate, DateTimeKind.Utc);


            var tableEntity = new TableEntity(_tenantId, reportDateUtc.ToString("yyyy-MM-dd"))
            {
                { "ReportRefreshDate", reportRefreshDateUtc },
                { "SiteType", tenantSnap.SiteType },
                { "StorageUsedInBytes", tenantSnap.StorageUsedInBytes }
            };

            try
            {
                // Try to add the entity if it doesn't exist
                await tableClient.AddEntityAsync(tableEntity);
            }
            catch (Azure.RequestFailedException ex) when (ex.Status == 409) // Conflict indicates the entity already exists
            {
                // Merge the entity if it already exists
                await tableClient.UpdateEntityAsync(tableEntity, ETag.All, TableUpdateMode.Merge);
            }
        }
    }
}
