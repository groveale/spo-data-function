using System.Text.Json.Serialization;

namespace groveale.Models
{
    public class SiteReport
    {
        [JsonPropertyName("reportRefreshDate")]
        public DateTime ReportRefreshDate { get; set; }

        [JsonPropertyName("siteId")]
        public string SiteId { get; set; }

        [JsonPropertyName("siteUrl")]
        public string SiteUrl { get; set; }

        [JsonPropertyName("ownerDisplayName")]
        public string OwnerDisplayName { get; set; }

        [JsonPropertyName("ownerPrincipalName")]
        public string OwnerPrincipalName { get; set; }

        [JsonPropertyName("isDeleted")]
        public bool IsDeleted { get; set; }

        [JsonPropertyName("lastActivityDate")]
        public DateTime? LastActivityDate { get; set; }

        [JsonPropertyName("siteSensitivityLabelId")]
        public string SiteSensitivityLabelId { get; set; }

        [JsonPropertyName("externalSharing")]
        public bool ExternalSharing { get; set; }

        [JsonPropertyName("unmanagedDevicePolicy")]
        public string UnmanagedDevicePolicy { get; set; }

        [JsonPropertyName("geolocation")]
        public string Geolocation { get; set; }

        [JsonPropertyName("fileCount")]
        public int FileCount { get; set; }

        [JsonPropertyName("activeFileCount")]
        public int ActiveFileCount { get; set; }

        [JsonPropertyName("pageViewCount")]
        public int PageViewCount { get; set; }

        [JsonPropertyName("visitedPageCount")]
        public int VisitedPageCount { get; set; }

        [JsonPropertyName("storageUsedInBytes")]
        public long StorageUsedInBytes { get; set; }

        [JsonPropertyName("storageAllocatedInBytes")]
        public long StorageAllocatedInBytes { get; set; }

        [JsonPropertyName("anonymousLinkCount")]
        public int AnonymousLinkCount { get; set; }

        [JsonPropertyName("companyLinkCount")]
        public int CompanyLinkCount { get; set; }

        [JsonPropertyName("secureLinkForGuestCount")]
        public int SecureLinkForGuestCount { get; set; }

        [JsonPropertyName("secureLinkForMemberCount")]
        public int SecureLinkForMemberCount { get; set; }

        [JsonPropertyName("rootWebTemplate")]
        public string RootWebTemplate { get; set; }

        [JsonPropertyName("reportPeriod")]
        public string ReportPeriod { get; set; }
    }
}