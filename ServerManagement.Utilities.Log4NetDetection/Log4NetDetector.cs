using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using ServerManagement.Utilities.LogDetection;

namespace ServerManagement.Utilities.Log4NetDetection
{
    public class Log4NetDetector : ILogDetector
    {
        public bool HasConfigFile(string configPath)
        {
            try
            {
                var directory = new FileInfo(configPath.Replace("\"", "")).Directory;
                if (directory == null)
                    return false;
                return directory.Exists && File.Exists(Path.Combine(directory.FullName, "log4net.config"));
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string GetLogFilePath(string configPath)
        {
            try
            {
                var directory = new FileInfo(configPath.Replace("\"", "")).Directory;
                if (directory == null)
                    return string.Empty;

                var xd = XDocument.Load(Path.Combine(directory.FullName, "log4net.config"));
                var logFileName = ((IEnumerable)xd
                    .XPathEvaluate("/log4net/appender[@type=\"log4net.Appender.RollingFileAppender\"]/file/@value"))
                    .Cast<XAttribute>()
                    .FirstOrDefault()?
                    .Value;

                return Path.GetFullPath(logFileName, directory.FullName);
            }
            catch (Exception e)
            {
                return e.GetBaseException().Message;
            }
        }
    }
}
