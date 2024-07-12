using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace groveale.Models
{
    public class Detail
    {
        [JsonPropertyName("reportPeriod")]
        public int ReportPeriod { get; set; }
        
        [JsonPropertyName("windows")]
        public bool Windows { get; set; }
        
        [JsonPropertyName("mac")]
        public bool Mac { get; set; }
        
        [JsonPropertyName("mobile")]
        public bool Mobile { get; set; }
        
        [JsonPropertyName("web")]
        public bool Web { get; set; }
        
        [JsonPropertyName("outlook")]
        public bool Outlook { get; set; }
        
        [JsonPropertyName("word")]
        public bool Word { get; set; }
        
        [JsonPropertyName("excel")]
        public bool Excel { get; set; }
        
        [JsonPropertyName("powerPoint")]
        public bool PowerPoint { get; set; }
        
        [JsonPropertyName("oneNote")]
        public bool OneNote { get; set; }
        
        [JsonPropertyName("teams")]
        public bool Teams { get; set; }
        
        [JsonPropertyName("outlookWindows")]
        public bool OutlookWindows { get; set; }
        
        [JsonPropertyName("wordWindows")]
        public bool WordWindows { get; set; }
        
        [JsonPropertyName("excelWindows")]
        public bool ExcelWindows { get; set; }
        
        [JsonPropertyName("powerPointWindows")]
        public bool PowerPointWindows { get; set; }
        
        [JsonPropertyName("oneNoteWindows")]
        public bool OneNoteWindows { get; set; }
        
        [JsonPropertyName("teamsWindows")]
        public bool TeamsWindows { get; set; }
        
        [JsonPropertyName("outlookMac")]
        public bool OutlookMac { get; set; }
        
        [JsonPropertyName("wordMac")]
        public bool WordMac { get; set; }
        
        [JsonPropertyName("excelMac")]
        public bool ExcelMac { get; set; }
        
        [JsonPropertyName("powerPointMac")]
        public bool PowerPointMac { get; set; }
        
        [JsonPropertyName("oneNoteMac")]
        public bool OneNoteMac { get; set; }
        
        [JsonPropertyName("teamsMac")]
        public bool TeamsMac { get; set; }
        
        [JsonPropertyName("outlookMobile")]
        public bool OutlookMobile { get; set; }
        
        [JsonPropertyName("wordMobile")]
        public bool WordMobile { get; set; }
        
        [JsonPropertyName("excelMobile")]
        public bool ExcelMobile { get; set; }
        
        [JsonPropertyName("powerPointMobile")]
        public bool PowerPointMobile { get; set; }
        
        [JsonPropertyName("oneNoteMobile")]
        public bool OneNoteMobile { get; set; }
        
        [JsonPropertyName("teamsMobile")]
        public bool TeamsMobile { get; set; }
        
        [JsonPropertyName("outlookWeb")]
        public bool OutlookWeb { get; set; }
        
        [JsonPropertyName("wordWeb")]
        public bool WordWeb { get; set; }
        
        [JsonPropertyName("excelWeb")]
        public bool ExcelWeb { get; set; }
        
        [JsonPropertyName("powerPointWeb")]
        public bool PowerPointWeb { get; set; }
        
        [JsonPropertyName("oneNoteWeb")]
        public bool OneNoteWeb { get; set; }
        
        [JsonPropertyName("teamsWeb")]
        public bool TeamsWeb { get; set; }

        public override string ToString()
        {
            // Convert boolean properties to "Yes" or "No"
            string isWindowsStr = Windows ? "Yes" : "No";
            string isMacStr = Mac ? "Yes" : "No";
            string isMobileStr = Mobile ? "Yes" : "No";
            string isWebStr = Web ? "Yes" : "No";
            string isOutlookStr = Outlook ? "Yes" : "No";
            string isWordStr = Word ? "Yes" : "No";
            string isExcelStr = Excel ? "Yes" : "No";
            string isPowerPointStr = PowerPoint ? "Yes" : "No";
            string isOneNoteStr = OneNote ? "Yes" : "No";
            string isTeamsStr = Teams ? "Yes" : "No";
            string isOutlookWindowsStr = OutlookWindows ? "Yes" : "No";
            string isWordWindowsStr = WordWindows ? "Yes" : "No";
            string isExcelWindowsStr = ExcelWindows ? "Yes" : "No";
            string isPowerPointWindowsStr = PowerPointWindows ? "Yes" : "No";
            string isOneNoteWindowsStr = OneNoteWindows ? "Yes" : "No";
            string isTeamsWindowsStr = TeamsWindows ? "Yes" : "No";
            string isOutlookMacStr = OutlookMac ? "Yes" : "No";
            string isWordMacStr = WordMac ? "Yes" : "No";
            string isExcelMacStr = ExcelMac ? "Yes" : "No";
            string isPowerPointMacStr = PowerPointMac ? "Yes" : "No";
            string isOneNoteMacStr = OneNoteMac ? "Yes" : "No";
            string isTeamsMacStr = TeamsMac ? "Yes" : "No";
            string isOutlookMobileStr = OutlookMobile ? "Yes" : "No";
            string isWordMobileStr = WordMobile ? "Yes" : "No";
            string isExcelMobileStr = ExcelMobile ? "Yes" : "No";
            string isPowerPointMobileStr = PowerPointMobile ? "Yes" : "No";
            string isOneNoteMobileStr = OneNoteMobile ? "Yes" : "No";
            string isTeamsMobileStr = TeamsMobile ? "Yes" : "No";
            string isOutlookWebStr = OutlookWeb ? "Yes" : "No";
            string isWordWebStr = WordWeb ? "Yes" : "No";
            string isExcelWebStr = ExcelWeb ? "Yes" : "No";
            string isPowerPointWebStr = PowerPointWeb ? "Yes" : "No";
            string isOneNoteWebStr = OneNoteWeb ? "Yes" : "No";
            string isTeamsWebStr = TeamsWeb ? "Yes" : "No";

            // Format the string as needed
            return $"{ReportPeriod},{isWindowsStr},{isMacStr},{isMobileStr},{isWebStr},{isOutlookStr},{isWordStr},{isExcelStr},{isPowerPointStr},{isOneNoteStr},{isTeamsStr},{isOutlookWindowsStr},{isWordWindowsStr},{isExcelWindowsStr},{isPowerPointWindowsStr},{isOneNoteWindowsStr},{isTeamsWindowsStr},{isOutlookMacStr},{isWordMacStr},{isExcelMacStr},{isPowerPointMacStr},{isOneNoteMacStr},{isTeamsMacStr},{isOutlookMobileStr},{isWordMobileStr},{isExcelMobileStr},{isPowerPointMobileStr},{isOneNoteMobileStr},{isTeamsMobileStr},{isOutlookWebStr},{isWordWebStr},{isExcelWebStr},{isPowerPointWebStr},{isOneNoteWebStr},{isTeamsWebStr}";
        }
    }

    public class M365AppUsageReport
    {
        [JsonPropertyName("reportRefreshDate")]
        public string ReportRefreshDate { get; set; }
        
        [JsonPropertyName("userPrincipalName")]
        public string UserPrincipalName { get; set; }
        
        [JsonPropertyName("lastActivationDate")]
        public string LastActivationDate { get; set; }
        
        [JsonPropertyName("lastActivityDate")]
        public string LastActivityDate { get; set; }
        
        [JsonPropertyName("details")]
        public List<Detail> Details { get; set; }
    }
}
