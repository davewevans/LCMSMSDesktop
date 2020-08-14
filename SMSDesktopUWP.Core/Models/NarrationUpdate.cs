using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMSDesktopUWP.Core.Models
{
    public class NarrationUpdate
    {
        public string Subject { get; set; }

        public string Note { get; set; }

        public DateTime EntryDate { get; set; }

        public int? OrphanID { get; set; }

        public int? GuardianID { get; set; }
    }
}
