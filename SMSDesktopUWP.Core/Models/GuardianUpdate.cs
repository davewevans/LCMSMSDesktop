using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMSDesktopUWP.Core.Models
{
    public class GuardianUpdate
    {
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public DateTime EntryDate { get; set; }

        public string Location { get; set; }
    }
}
