using ServerManagement.Core.Responses.Storage;
using System;
using System.Collections.Generic;

namespace ServerManagement.Core.Requests.Storage
{
    public class GetDrivesRequest : MediatR.IRequest<List<DiskDrive>>
    {
        public string ComputerName { get; set; } = Environment.MachineName;
    }
}
