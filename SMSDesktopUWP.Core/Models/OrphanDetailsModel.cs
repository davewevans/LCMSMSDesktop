using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SMSDesktopUWP.Core.Models
{
    public class OrphanDetailsModel
    {
        public int OrphanID { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "Too many characters.")]
        public string FirstName { get; set; } = string.Empty;

        [StringLength(255, ErrorMessage = "Too many characters.")]
        public string MiddleName { get; set; } = string.Empty;

        [Required]
        [StringLength(255, ErrorMessage = "Too many characters.")]
        public string LastName { get; set; } = string.Empty;

        public string FullName => $"{FirstName} {MiddleName} {LastName}";

        public string Gender { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string LCMStatus { get; set; }

        [StringLength(255, ErrorMessage = "Too many characters.")]
        public string ProfileNumber { get; set; }

        public DateTime EntryDate { get; set; }

        public int? GuardianID { get; set; }

        public int? ProfilePictureID { get; set; }

        public string ProfilePicUri { get; set; }

        public Picture ProfilePic { get; set; }

        public List<Picture> Pictures { get; set; }

        public List<Sponsor> Sponsors { get; set; }

        public Guardian Guardian { get; set; }

        public List<Narration> Narrations { get; set; }

        public List<Academic> Academics { get; set; }
    }
}
