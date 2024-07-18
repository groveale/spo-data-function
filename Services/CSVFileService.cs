using System.Text;
using groveale.Models;

namespace groveale.Services 
{
    public interface ICSVFileService
    {
        Task<byte[]> ConvertM365ReportToCsvAndReturnAsBytesAsync(List<M365AppUsageReport> reportData);
        Task<byte[]> ConvertStringToCsvAndReturnAsBytesAsync(string reportData);
    }

    public class CSVFileService : ICSVFileService
    {
        public async Task<byte[]> ConvertM365ReportToCsvAndReturnAsBytesAsync(List<M365AppUsageReport> reportData)
        {
            // Step 1: Convert objects to CSV format
            var csvBuilder = new StringBuilder();
            // Add CSV header
            csvBuilder.AppendLine("ReportRefreshDate,UserPrincipalName,LastActivationDate,LastActivityDate,ReportPeriod,Windows,Mac,Mobile,Web,Outlook,Word,Excel,PowerPoint,OneNote,Teams,OutlookWindows,WordWindows,ExcelWindows,PowerPointWindows,OneNoteWindows,TeamsWindows,OutlookMac,WordMac,ExcelMac,PowerPointMac,OneNoteMac,TeamsMac,OutlookMobile,WordMobile,ExcelMobile,PowerPointMobile,OneNoteMobile,TeamsMobile,OutlookWeb,WordWeb,ExcelWeb,PowerPointWeb,OneNoteWeb,TeamsWeb"); 

            foreach (var report in reportData)
            {
                //csvBuilder.AppendLine($"{report.ReportRefreshDate:yyyy-MM-dd},{report.UserPrincipalName},{report.LastActivationDate:yyyy-MM-dd},{report.LastActivityDate:yyyy-MM-dd},{report.Details.FirstOrDefault().ReportPeriod},{report.Details.FirstOrDefault().Windows},{report.Details.FirstOrDefault().Mac},{report.Details.FirstOrDefault().Mobile},{report.Details.FirstOrDefault().Web},{report.Details.FirstOrDefault().Outlook},{report.Details.FirstOrDefault().Word},{report.Details.FirstOrDefault().Excel},{report.Details.FirstOrDefault().PowerPoint},{report.Details.FirstOrDefault().OneNote},{report.Details.FirstOrDefault().Teams},{report.Details.FirstOrDefault().OutlookWindows},{report.Details.FirstOrDefault().WordWindows},{report.Details.FirstOrDefault().ExcelWindows},{report.Details.FirstOrDefault().PowerPointWindows},{report.Details.FirstOrDefault().OneNoteWindows},{report.Details.FirstOrDefault().TeamsWindows},{report.Details.FirstOrDefault().OutlookMac},{report.Details.FirstOrDefault().WordMac},{report.Details.FirstOrDefault().ExcelMac},{report.Details.FirstOrDefault().PowerPointMac},{report.Details.FirstOrDefault().OneNoteMac},{report.Details.FirstOrDefault().TeamsMac},{report.Details.FirstOrDefault().OutlookMobile},{report.Details.FirstOrDefault().WordMobile},{report.Details.FirstOrDefault().ExcelMobile},{report.Details.FirstOrDefault().PowerPointMobile},{report.Details.FirstOrDefault().OneNoteMobile},{report.Details.FirstOrDefault().TeamsMobile},{report.Details.FirstOrDefault().OutlookWeb},{report.Details.FirstOrDefault().WordWeb},{report.Details.FirstOrDefault().ExcelWeb},{report.Details.FirstOrDefault().PowerPointWeb},{report.Details.FirstOrDefault().OneNoteWeb},{report.Details.FirstOrDefault().TeamsWeb}");
                csvBuilder.AppendLine($"{report.ReportRefreshDate:yyyy-MM-dd},{report.UserPrincipalName},{report.LastActivationDate:yyyy-MM-dd},{report.LastActivityDate:yyyy-MM-dd},{report.Details.FirstOrDefault()}");
            }

            // Step 3: Write CSV string to MemoryStream
            using (var memoryStream = new MemoryStream())
            using (var streamWriter = new StreamWriter(memoryStream))
            {
                await streamWriter.WriteAsync(csvBuilder.ToString());
                await streamWriter.FlushAsync();
                return memoryStream.ToArray(); // This byte array can be used for uploading
            }
        }

        public async Task<byte[]> ConvertStringToCsvAndReturnAsBytesAsync(string reportData)
        {
            using (var memoryStream = new MemoryStream())
            using (var streamWriter = new StreamWriter(memoryStream))
            {
                await streamWriter.WriteAsync(reportData);
                await streamWriter.FlushAsync();
                return memoryStream.ToArray(); // This byte array can be used for uploading
            }
        }
    }
}