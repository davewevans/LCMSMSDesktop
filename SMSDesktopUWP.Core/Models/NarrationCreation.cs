using System;

namespace SMSDesktopUWP.Core.Models
{
    public class NarrationCreation 
    {
        public string Subject { get; set; }

        public string Note { get; set; }

        public DateTime EntryDate { get; set; }

        public int? OrphanID { get; set; }

        public int? GuardianID { get; set; }
    }
}   