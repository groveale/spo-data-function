using System.Text.Json;
using groveale.Models;

namespace groveale.Services 
{
    public interface IStorageSnapshotService
    {
        int ProcessSiteSnapshotAsync(List<SiteReport> siteSnapshots);
    }

    public class StorageSnapshotService : IStorageSnapshotService
    {
        public int ProcessSiteSnapshotAsync(List<SiteReport> siteSnapshots)
        {
            // Save to Table Storage
            return 0;
        }
    }
}
