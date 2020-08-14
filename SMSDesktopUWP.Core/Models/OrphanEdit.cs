using System;

namespace SMSDesktopUWP.Core.Models
{
    public class OrphanEdit
    {
        public string FirstName { get; set; } = string.Empty;

        public string MiddleName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Gender { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string LCMStatus { get; set; }

        public string ProfileNumber { get; set; }

        public int? GuardianID { get; set; }

        public DateTime EntryDate { get; set; }

        public int? ProfilePictureID { get; set; }


    }
}
