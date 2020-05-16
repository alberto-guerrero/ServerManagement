using ServerManagement.Detectors.Responses.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServerManagement.Detectors.Requests.Services
{
    public class DetectRequest : MediatR.IRequest<List<Service>>
    {
        public string ComputerName { get; set; } = Environment.MachineName;
    }
}
