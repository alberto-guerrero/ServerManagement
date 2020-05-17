using MediatR;
using ServerManagement.Core.Responses.Updates;
using System;
using System.Collections.Generic;

namespace ServerManagement.Core.Requests.Updates
{
    public class GetWindowsUpdateListRequest : IRequest<List<WindowsUpdate>>
    {
        public string ComputerName { get; set; } = Environment.MachineName;
    }
}
