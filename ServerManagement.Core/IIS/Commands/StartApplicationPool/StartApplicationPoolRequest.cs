using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Web.Administration;

namespace ServerManagement.Core.IIS.Commands.StartApplicationPool
{
    public class StartApplicationPoolRequest : IRequest<string>
    {
        public string ApplicationPoolName { get; set; }
    }

    public class StartApplicationPoolRequestHandler : IRequestHandler<StartApplicationPoolRequest, string>
    {
        public Task<string> Handle(StartApplicationPoolRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var manager = new ServerManager();
                var applicationPool = manager.ApplicationPools
                    .FirstOrDefault(p => p.Name == request.ApplicationPoolName);

                if (applicationPool == null)
                    return Task.FromResult("Not Found");

                if (applicationPool.State != ObjectState.Started)
                {
                    applicationPool.Start();
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
