namespace ServerManagement.Core.IIS.Queries.GetApplicationPools
{
    public class ApplicationPoolDto
    {
        public string Name { get; set; }
        public string Status { get; set; }
        public string ManagedPipeline { get; set; }
        public string RunTimeVersion { get; set; }
    }
}
