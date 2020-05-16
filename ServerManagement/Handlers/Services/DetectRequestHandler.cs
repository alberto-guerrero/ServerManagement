using MediatR;
using ServerManagement.Detectors.Requests.Services;
using ServerManagement.Detectors.Responses.Services;
using System.Collections.Generic;
using System.Management;
using System.Threading;
using System.Threading.Tasks;

namespace ServerManagement.Detectors.Handlers.Services
{
    public class DetectRequestHandler : IRequestHandler<DetectRequest, List<Service>>
    {
        public Task<List<Service>> Handle(DetectRequest request, CancellationToken cancellationToken)
        {
            var services = new List<Service>();

            var scope = new ManagementScope($@"\\{request.ComputerName}\root\CIMV2");
            var query = new ObjectQuery("SELECT * FROM Win32_Service");
            var searcher = new ManagementObjectSearcher(scope, query);

            // Retrieve a list of running services.
            foreach (ManagementObject m in searcher.Get())
            {
                services.Add(new Service
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
                });
            }

            return Task.FromResult(services);
        }
    }
}
