using MediatR;
using ServerManagement.Detectors.Requests.Updates;
using ServerManagement.Detectors.Responses.Updates;
using System;
using System.Collections.Generic;
using System.Management;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ServerManagement.Detectors.Handlers.Updates
{
    public class DetectRequestHandler : IRequestHandler<DetectRequest, List<WindowsUpdate>>
    {
        public Task<List<WindowsUpdate>> Handle(DetectRequest request, CancellationToken cancellationToken)
        {
            var updates = new List<WindowsUpdate>();

            const string microsoftKbUrl = "https://support.microsoft.com/en-us/help/";

            // Setup WMI query.
            var scope = new ManagementScope($@"\\{request.ComputerName}\root\CIMV2");
            var query = new ObjectQuery("SELECT HotFixID,InstalledOn FROM Win32_QuickFixEngineering WHERE HotFixID <> 'File 1'");
            var searcher = new ManagementObjectSearcher(scope, query);

            foreach (ManagementObject m in searcher.Get())
            {
                var update = new WindowsUpdate();

                update.UpdateId = m["HotFixID"]?.ToString() ?? string.Empty;

                if (DateTime.TryParse(m["InstalledOn"]?.ToString(), out DateTime installDate))
                    update.InstallDate = installDate;
                
                update.ExternalLink = (microsoftKbUrl + ExtractKbNumber(update.UpdateId)) ?? string.Empty;

                if (!string.IsNullOrEmpty(update.UpdateId))
                    updates.Add(update);
            }

            return Task.FromResult(updates);
        }

        private string ExtractKbNumber(string updateId)
        {
            return new Regex(@"^KB(?<kbNumber>\d{5,8})")
                ?.Match(updateId)
                ?.Groups["kbNumber"]
                ?.Value;
        }
    }
}