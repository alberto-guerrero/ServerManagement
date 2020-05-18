using System.ServiceProcess;

namespace ServerManagement.Core.Requests.Services
{
    public class StartServiceRequest : MediatR.IRequest<ServiceControllerStatus>
    {
        public string ServiceName { get; set; }
    }
}
