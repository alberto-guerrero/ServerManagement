using System.ServiceProcess;

namespace ServerManagement.Core.Requests.Services
{
    public class StopServiceRequest : MediatR.IRequest<ServiceControllerStatus>
    {
        public string ServiceName { get; set; }
    }
}
