using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Threading;
using System.Threading.Tasks;

namespace ServerManagement.Core.WindowsUpdates.Queries.GetWindowsUpdates
{
    public class GetWindowsUpdateListRequest : IRequest<List<WindowsUpdateDto>>
    {
        public string ComputerName { get; set; } = Environment.MachineName;
    }

    public class GetWindowsUpdateListHandler : IRequestHandler<GetWindowsUpdateListRequest, List<WindowsUpdateDto>>
    {
        public Task<List<WindowsUpdateDto>> Handle(GetWindowsUpdateListRequest request, CancellationToken cancellationToken)
        {
            // Setup WMI query.
            var scope = new ManagementScope($@"\\{request.ComputerName}\root\CIMV2");
            var query = new ObjectQuery("SELECT HotFixID,InstalledOn FROM Win32_QuickFixEngineering WHERE HotFixID <> 'File 1'");
            var searcher = new ManagementObjectSearcher(scope, query);

            var updates = searcher
                .Get()
                .Cast<ManagementObject>()
                .Select(m => new WindowsUpdateDto
                {
                    UpdateId = m["HotFixID"]?.ToString() ?? string.Empty,
                    InstallDate = DateTime.TryParse(m["InstalledOn"]?.ToString(), out DateTime installDate)
                                        ? installDate
                                        : default
                })
                .Where(update => !string.IsNullOrEmpty(update.UpdateId))
                .ToList();

            return Task.FromResult(updates);
        }
    }
}