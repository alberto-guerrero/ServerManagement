namespace ServerManagement.Utilities.LogDetection
{
    public interface ILogDetector
    {
        bool HasConfigFile(string configPath);
        string GetLogFilePath(string configPath);
    }
}
