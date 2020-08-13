using System;
using System.Collections.Generic;
using System.Text;

namespace SMSDesktopUWP.Core.Models
{
    public class OrphanStatistics
    {
        public int ActiveCount { get; set; }

        public int InactiveCount { get; set; }

        public int UnknownCount { get; set; }

        public int TotalCount { get; set; }
    }
}
