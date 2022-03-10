using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Web.Administration;

namespace ServerManagement.Core.IIS.Commands.RecycleApplicationPool
{
    public class RecycleApplicationPoolRequest : IRequest<string>
    {
        public string ApplicationPoolName { get; set; }
    }

    public class RecycleApplicationPoolRequestHandler : IRequestHandler<RecycleApplicationPoolRequest, string>
    {
        public Task<string> Handle(RecycleApplicationPoolRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var manager = new ServerManager();
                var applicationPool = manager.ApplicationPools
                    .FirstOrDefault(p => p.Name == request.ApplicationPoolName);

                if (applicationPool == null)
                    return Task.FromResult("Not Found");

                if (applicationPool.State == ObjectState.Started)
                {
                    applicationPool.Recycle();
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
