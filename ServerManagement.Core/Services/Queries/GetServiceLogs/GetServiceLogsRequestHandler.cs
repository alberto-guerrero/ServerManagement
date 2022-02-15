using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ServerManagement.Utilities.LogDetection;

namespace ServerManagement.Core.Services.Queries.GetServiceLogs
{
    public class GetServiceLogs : IRequest<string>
    {
        public string ServicePath { get; set; }
    }

    public class GetServiceLogsRequestHandler : IRequestHandler<GetServiceLogs, string>
    {
        private readonly ILogDetector _logDetector;

        public GetServiceLogsRequestHandler(ILogDetector logDetector)
        {
            _logDetector = logDetector;
        }

        public Task<string> Handle(GetServiceLogs request, CancellationToken cancellationToken)
        {
            var logFile = _logDetector.GetLogFilePath(request.ServicePath);
            return File.ReadAllTextAsync(logFile, cancellationToken);
        }
    }
}
