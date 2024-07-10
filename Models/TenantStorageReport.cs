using System.Text.Json.Serialization;
namespace groveale.Models 
{
    public class TenantStorageReport
    {
        [JsonPropertyName("reportRefreshDate")]
        public DateTime ReportRefreshDate { get; set; }

        [JsonPropertyName("siteType")]
        public string SiteType { get; set; }

        [JsonPropertyName("storageUsedInBytes")]
        public long StorageUsedInBytes { get; set; }

        [JsonPropertyName("reportDate")]
        public DateTime ReportDate { get; set; }

        [JsonPropertyName("reportPeriod")]
        public string ReportPeriod { get; set; }
    }

}