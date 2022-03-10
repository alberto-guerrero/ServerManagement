using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Web.Administration;

namespace ServerManagement.Core.IIS.Commands.StopApplicationPool
{
    public class StopApplicationPoolRequest : IRequest<string>
    {
        public string ApplicationPoolName { get; set; }
    }

    public class StopApplicationPoolRequestHandler : IRequestHandler<StopApplicationPoolRequest, string>
    {
        public Task<string> Handle(StopApplicationPoolRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var manager = new ServerManager();
                var applicationPool = manager.ApplicationPools
                    .FirstOrDefault(p => p.Name == request.ApplicationPoolName);

                if (applicationPool == null)
                    return Task.FromResult("Not Found");

                if (applicationPool.State != ObjectState.Stopped)
                {
                    applicationPool.Stop();
                }

                // Return current status
                return Task.FromResult(Enum.GetName(typeof(ObjectState), applicationPool.State));
            }
            catch (Exception)
            {
                return Task.FromResult("Error");
            }
        }
    }
}
