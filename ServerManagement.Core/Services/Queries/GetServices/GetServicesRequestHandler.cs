using MediatR;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Threading;
using System.Threading.Tasks;

namespace ServerManagement.Core.Services.Queries.GetServices
{
    public class GetServices : MediatR.IRequest<List<ServiceDto>>
    {
        public string ComputerName { get; set; } = Environment.MachineName;

        public Func<ServiceDto, bool> Filter { get; set; } = (service) => true;
    }

    public class GetServicesRequestHandler : IRequestHandler<GetServices, List<ServiceDto>>
    {
        public Task<List<ServiceDto>> Handle(GetServices request, CancellationToken cancellationToken)
        {
            // Setup WMI Query
            var scope = new ManagementScope($@"\\{request.ComputerName}\root\CIMV2");
            var query = new ObjectQuery("SELECT * FROM Win32_Service");
            var searcher = new ManagementObjectSearcher(scope, query);

            // Retrieve a list of running services.
            var services = searcher
                .Get()
                .Cast<ManagementObject>()
                .Select(m => new ServiceDto
                {
                    DisplayName = m["DisplayName"]?.ToString() ?? string.Empty,
                    AcceptPause = (bool)(m["AcceptPause"] ?? false),
                    AcceptStop = (bool)(m["AcceptStop"] ?? false),
                    Description = m["Description"]?.ToString() ?? string.Empty,
                    Name = m["Name"]?.ToString() ?? string.Empty,
                    PathName = m["PathName"]?.ToString() ?? string.Empty,
                    StartupType = m["StartMode"]?.ToString() ?? string.Empty,
                    LogOnAs = m["StartName"]?.ToString() ?? string.Empty,
                    State = m["State"]?.ToString() ?? string.Empty
                })
                .Where(request.Filter)
                .ToList();

            return Task.FromResult(services);
        }
    }
}
