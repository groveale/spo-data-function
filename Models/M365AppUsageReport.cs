using System;
using System.Collections.Generic;

public class M365AppUsageReport
{
    public DateTime ReportRefreshDate { get; set; }
    public string UserPrincipalName { get; set; }
    public DateTime LastActivationDate { get; set; }
    public DateTime LastActivityDate { get; set; }
    public List<Detail> Details { get; set; }
}

public class Detail
{
    public int ReportPeriod { get; set; }
    public bool Windows { get; set; }
    public bool Mac { get; set; }
    public bool Mobile { get; set; }
    public bool Web { get; set; }
    public bool Outlook { get; set; }
    public bool Word { get; set; }
    public bool Excel { get; set; }
    public bool PowerPoint { get; set; }
    public bool OneNote { get; set; }
    public bool Teams { get; set; }
    public bool OutlookWindows { get; set; }
    public bool WordWindows { get; set; }
    public bool ExcelWindows { get; set; }
    public bool PowerPointWindows { get; set; }
    public bool OneNoteWindows { get; set; }
    public bool TeamsWindows { get; set; }
    public bool OutlookMac { get; set; }
    public bool WordMac { get; set; }
    public bool ExcelMac { get; set; }
    public bool PowerPointMac { get; set; }
    public bool OneNoteMac { get; set; }
    public bool TeamsMac { get; set; }
    public bool OutlookMobile { get; set; }
    public bool WordMobile { get; set; }
    public bool ExcelMobile { get; set; }
    public bool PowerPointMobile { get; set; }
    public bool OneNoteMobile { get; set; }
    public bool TeamsMobile { get; set; }
    public bool OutlookWeb { get; set; }
    public bool WordWeb { get; set; }
    public bool ExcelWeb { get; set; }
    public bool PowerPointWeb { get; set; }
    public bool OneNoteWeb { get; set; }
    public bool TeamsWeb { get; set; }
}
