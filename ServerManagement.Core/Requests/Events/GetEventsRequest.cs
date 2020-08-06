using MediatR;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ServerManagement.Core.Requests.Events
{
    public class GetEventsRequest : IRequest<List<EventLog>>
    {
        public string ComputerName { get; set; } = Environment.MachineName;
    }
}
