using ServerManagement.Core.Responses.Services;
using System;
using System.CodeDom;
using System.Collections.Generic;

namespace ServerManagement.Core.Requests.Services
{
    public class GetServiceList : MediatR.IRequest<List<Service>>
    {
        public string ComputerName { get; set; } = Environment.MachineName;

        public Func<Service, bool> Filter { get; set; } = (service) => true;
    }
}
