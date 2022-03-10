using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Web.Administration;

namespace ServerManagement.Core.IIS.Queries.GetApplicationPools
{
    public class GetApplicationPool : IRequest<List<ApplicationPoolDto>>
    {
        public Func<ApplicationPoolDto, bool> Filter { get; set; } = pool => true;
    }

    public class GetApplicationPoolRequestHandler : IRequestHandler<GetApplicationPool, List<ApplicationPoolDto>>
    {
        public Task<List<ApplicationPoolDto>> Handle(GetApplicationPool request, CancellationToken cancellationToken)
        {
            var manager = new ServerManager();

            return Task.FromResult(manager.ApplicationPools.Select(p => new ApplicationPoolDto
            {
                Name = p.Name,
                Status = Enum.GetName(typeof(ObjectState),p.State),
                ManagedPipeline = Enum.GetName(typeof(ManagedPipelineMode), p.ManagedPipelineMode),
                RunTimeVersion = p.ManagedRuntimeVersion
            }).Where(request.Filter).ToList());
        }
    }
}
