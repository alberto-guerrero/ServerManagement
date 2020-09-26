namespace ServerManagement.Core.Storage.Queries.GetDrives
{
    public class DiskDriveDto
    {
        public string DriveLetter { get; set; }
        public ulong Capacity { get; set; }
        public string CapacityString { get; set; }
        public ulong FreeSpace { get; set; }
        public string FreeSpaceString { get; set; }
        public ulong UsedSpace { get; set; }
        public string UsedSpaceString { get; set; }
        public double UsedSpacePercentage => (double)UsedSpace / Capacity;
        public uint DriveType { get; set; }
        public string VolumeName { get; set; }
    }
}
