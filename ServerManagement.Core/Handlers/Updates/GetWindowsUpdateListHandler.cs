using MediatR;
using ServerManagement.Core.Requests.Updates;
using ServerManagement.Core.Responses.Updates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Threading;
using System.Threading.Tasks;

namespace ServerManagement.Core.Handlers.Updates
{
    public class GetWindowsUpdateListHandler : IRequestHandler<GetWindowsUpdateListRequest, List<WindowsUpdate>>
    {
        public Task<List<WindowsUpdate>> Handle(GetWindowsUpdateListRequest request, CancellationToken cancellationToken)
        {        
            // Setup WMI query.
            var scope = new ManagementScope($@"\\{request.ComputerName}\root\CIMV2");
            var query = new ObjectQuery("SELECT HotFixID,InstalledOn FROM Win32_QuickFixEngineering WHERE HotFixID <> 'File 1'");
            var searcher = new ManagementObjectSearcher(scope, query);

            var updates = searcher
                .Get()
                .Cast<ManagementObject>()
                .Select(m => new WindowsUpdate
                {
                    UpdateId = m["HotFixID"]?.ToString() ?? string.Empty,
                    InstallDate = (DateTime.TryParse(m["InstalledOn"]?.ToString(), out DateTime installDate))
                                        ? installDate
                                        : default
                })
                .Where(update => !string.IsNullOrEmpty(update.UpdateId))
                .ToList();

            return Task.FromResult(updates);
        }
    }
}