using MediatR;
using ServerManagement.Core.Requests.Services;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;

namespace ServerManagement.Core.Handlers.Services
{
    public class StartServiceRequestHandler : IRequestHandler<StartServiceRequest, ServiceControllerStatus>
    {
        public Task<ServiceControllerStatus> Handle(StartServiceRequest request, CancellationToken cancellationToken)
        {
            // Note:  When debugging in VS, remember to run VS as admin to not throw "Access Denied" message.

            // Start Service
            var sc = new ServiceController(request.ServiceName);
            
            // Only start if it's not already running
            if (sc.Status != ServiceControllerStatus.Running)
                sc.Start();
            
            // Return current status
            return Task.FromResult(sc.Status);
        }
    }
}