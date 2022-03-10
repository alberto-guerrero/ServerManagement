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

        public async Task<string> Handle(GetServiceLogs request, CancellationToken cancellationToken)
        {
            var logFile = _logDetector.GetLogFilePath(request.ServicePath);

            var logFileStream = new FileStream(logFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var logFileReader = new StreamReader(logFileStream);

            var content = await logFileReader.ReadToEndAsync();

            logFileReader.Close();
            logFileStream.Close();
            return content;
        }
    }
}
