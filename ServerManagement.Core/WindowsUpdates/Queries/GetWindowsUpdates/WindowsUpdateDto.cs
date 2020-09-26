using System;
using System.Text.RegularExpressions;

namespace ServerManagement.Core.WindowsUpdates.Queries.GetWindowsUpdates
{
    public class WindowsUpdateDto
    {
        const string microsoftKbUrl = "https://support.microsoft.com/en-us/help/";

        public string UpdateId { get; set; }
        public DateTime InstallDate { get; set; }
        public string ExternalLink => microsoftKbUrl + ExtractKbNumber(UpdateId) ?? string.Empty;

        private string ExtractKbNumber(string updateId)
        {
            return new Regex(@"^KB(?<kbNumber>\d{5,8})")
                ?.Match(updateId)
                ?.Groups["kbNumber"]
                ?.Value;
        }
    }
}
