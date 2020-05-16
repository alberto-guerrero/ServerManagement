using System;
using System.Collections.Generic;
using System.Text;

namespace ServerManagement.Detectors.Responses.Updates
{
    public class WindowsUpdate
    {
        public string UpdateId { get; set; }
        public string ExternalLink { get; set; }
        public DateTime InstallDate { get; set; }
    }
}
