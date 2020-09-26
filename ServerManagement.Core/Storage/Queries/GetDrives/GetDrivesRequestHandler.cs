using MediatR;
using System;
using System.Collections.Generic;
using System.Management;
using System.Threading;
using System.Threading.Tasks;

namespace ServerManagement.Core.Storage.Queries.GetDrives
{
    public class GetDrivesRequest : MediatR.IRequest<List<DiskDriveDto>>
    {
        public string ComputerName { get; set; } = Environment.MachineName;
    }

    public class GetDrivesRequestHandler : IRequestHandler<GetDrivesRequest, List<DiskDriveDto>>
    {
        public Task<List<DiskDriveDto>> Handle(GetDrivesRequest request, CancellationToken cancellationToken)
        {
            // Use WMI to retrieve a list of storage devices.
            var drives = new List<DiskDriveDto>();

            // Setup WMI query.
            var scope = new ManagementScope($@"\\{request.ComputerName}\root\CIMV2");
            var query = new ObjectQuery("SELECT * FROM Win32_LogicalDisk WHERE DriveType = 2 OR DriveType = 3 OR DriveType = 5");
            var searcher = new ManagementObjectSearcher(scope, query);

            // Retrieve a list of storage devices.
            foreach (ManagementObject m in searcher.Get())
            {
                var drive = new DiskDriveDto();

                drive.DriveLetter = m["Name"]?.ToString() ?? string.Empty;
                drive.VolumeName = m["VolumeName"]?.ToString() ?? string.Empty;
                drive.Capacity = (ulong?)m["Size"] ?? 0;
                drive.FreeSpace = m["FreeSpace"] != null ? (ulong)m["FreeSpace"] : 0;
                drive.UsedSpace = drive.Capacity - drive.FreeSpace;
                drive.DriveType = (uint)m["DriveType"];

                double bytes = drive.Capacity;
                switch (drive.DriveType)
                {
                    case 2:
                        drive.CapacityString = "Removable";
                        break;
                    case 5:
                        drive.CapacityString = "CD-ROM";
                        break;
                    default:
                        drive.CapacityString = ConvertBytesToString(bytes);
                        break;
                }

                bytes = drive.FreeSpace;
                drive.FreeSpaceString = drive.DriveType == 2 || drive.DriveType == 5 ? string.Empty : ConvertBytesToString(bytes);

                bytes = drive.UsedSpace;
                drive.UsedSpaceString = drive.DriveType == 2 || drive.DriveType == 5 ? string.Empty : ConvertBytesToString(bytes);

                drives.Add(drive);
            }

            return Task.FromResult(drives);
        }

        private static string ConvertBytesToString(double bytes)
        {
            string suffix = "KB";
            bytes /= 1024.0;
            if (bytes >= 1000.0)
            {
                bytes /= 1024.0;
                suffix = "MB";
            }
            if (bytes >= 1000.0)
            {
                bytes /= 1024.0;
                suffix = "GB";
            }
            if (bytes >= 1000.0)
            {
                bytes /= 1024.0;
                suffix = "TB";
            }

            return $"{bytes:N1} {suffix}";
        }
    }
}
