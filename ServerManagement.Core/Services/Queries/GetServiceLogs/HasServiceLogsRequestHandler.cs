using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ServerManagement.Utilities.LogDetection;

namespace ServerManagement.Core.Services.Queries.GetServiceLogs
{
    public class HasServiceLogs : IRequest<bool>
    {
        public string ServicePath { get; set; }
    }
    public class HasServiceLogsRequestHandler : IRequestHandler<HasServiceLogs, bool>
    {
        private readonly ILogDetector _logDetector;

        public HasServiceLogsRequestHandler(ILogDetector logDetector)
        {
            _logDetector = logDetector;
        }

        public Task<bool> Handle(HasServiceLogs request, CancellationToken cancellationToken) => Task.FromResult(_logDetector.HasConfigFile(request.ServicePath));
    }
}
