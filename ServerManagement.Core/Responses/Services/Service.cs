﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ServerManagement.Core.Responses.Services
{
    public class Service
    {
        public string DisplayName { get; internal set; }
        public bool AcceptPause { get; internal set; }
        public bool AcceptStop { get; internal set; }
        public string Description { get; internal set; }
        public string Name { get; internal set; }
        public string PathName { get; internal set; }
        public string StartupType { get; internal set; }
        public string LogOnAs { get; internal set; }
        public string State { get; set; }
    }
}
