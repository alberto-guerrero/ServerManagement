using ServerManagement.Detectors.Responses.Services;
using System;
using System.Collections.Generic;

namespace ServerManagement.Detectors.Requests.Services
{
    public class GetServiceList : MediatR.IRequest<List<Service>>
    {
        public string ComputerName { get; set; } = Environment.MachineName;
    }
}
