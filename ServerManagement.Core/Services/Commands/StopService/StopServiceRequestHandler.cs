using MediatR;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using static System.ServiceProcess.ServiceControllerStatus;

namespace ServerManagement.Core.Services.Commands.StopService
{
    public class StopServiceRequest : MediatR.IRequest<ServiceControllerStatus>
    {
        public string ServiceName { get; set; }
    }

    public class StopServiceRequestHandler : IRequestHandler<StopServiceRequest, ServiceControllerStatus>
    {
        public Task<ServiceControllerStatus> Handle(StopServiceRequest request, CancellationToken cancellationToken)
        {
            // Note:  When debugging in VS, remember to run VS as admin to not throw "Access Denied" message.

            var sc = new ServiceController(request.ServiceName);

            if (sc.Status != Stopped && sc.Status != StopPending)
            {
                sc.Stop();
            }

            // Return current status
            return Task.FromResult(sc.Status);
        }
    }
}