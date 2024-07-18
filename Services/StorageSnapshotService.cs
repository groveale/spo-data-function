using System.Text.Json;
using Azure;
using Azure.Data.Tables;
using groveale.Models;

namespace groveale.Services 
{
    public interface IStorageSnapshotService
    {
        Task ProcessTenantSnapshot(TenantStorageReport tenantSnap);
        Task ProcessSiteSnapshots(List<SiteReport> siteSnapshots);
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

        public async Task ProcessSiteSnapshots(List<SiteReport> siteSnapshots)
        {
            var tableClient = _serviceClient.GetTableClient(_siteSnapTableName);
            tableClient.CreateIfNotExists();

            foreach (var siteSnap in siteSnapshots)
            {
                // Ensure the ReportRefreshDate is specified as UTC
                DateTime reportRefreshDateUtc = DateTime.SpecifyKind(siteSnap.ReportRefreshDate, DateTimeKind.Utc);

                // Ensure the LastActivityDate is specified as UTC
                if (siteSnap.LastActivityDate.HasValue)
                {
                    siteSnap.LastActivityDate = DateTime.SpecifyKind(siteSnap.LastActivityDate.Value, DateTimeKind.Utc);
                }

                var tableEntity = new TableEntity(siteSnap.SiteId, reportRefreshDateUtc.ToString("yyyy-MM-dd"))
                {
                    { "SiteUrl", siteSnap.SiteUrl },
                    { "OwnerDisplayName", siteSnap.OwnerDisplayName },
                    { "OwnerPrincipalName", siteSnap.OwnerPrincipalName },
                    { "IsDeleted", siteSnap.IsDeleted },
                    { "LastActivityDate", siteSnap.LastActivityDate },
                    { "SiteSensitivityLabelId", siteSnap.SiteSensitivityLabelId },
                    { "ExternalSharing", siteSnap.ExternalSharing },
                    { "UnmanagedDevicePolicy", siteSnap.UnmanagedDevicePolicy },
                    { "Geolocation", siteSnap.Geolocation },
                    { "FileCount", siteSnap.FileCount },
                    { "ActiveFileCount", siteSnap.ActiveFileCount },
                    { "PageViewCount", siteSnap.PageViewCount },
                    { "VisitedPageCount", siteSnap.VisitedPageCount },
                    { "StorageUsedInBytes", siteSnap.StorageUsedInBytes },
                    { "StorageAllocatedInBytes", siteSnap.StorageAllocatedInBytes },
                    { "AnonymousLinkCount", siteSnap.AnonymousLinkCount },
                    { "CompanyLinkCount", siteSnap.CompanyLinkCount },
                    { "SecureLinkForGuestCount", siteSnap.SecureLinkForGuestCount },
                    { "SecureLinkForInternalCount", siteSnap.SecureLinkForMemberCount }

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
}
