using MediatR;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ServerManagement.Core.EventViewer.Queries.GetEvents
{
    public class GetEventsRequest : IRequest<List<EventLog>>
    {
        public string ComputerName { get; set; } = Environment.MachineName;
    }

    public class GetEventsRequestHandler : IRequestHandler<GetEventsRequest, List<EventLog>>
    {
        public Task<List<EventLog>> Handle(GetEventsRequest request, CancellationToken cancellationToken)
        {
            var events = EventLog.GetEventLogs(request.ComputerName)
                .ToList();

            return Task.FromResult(events);
        }
    }
}
