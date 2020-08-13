using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SMSDesktopUWP.Core.Models
{
    public class Orphan
    {
        [Key]
        public int OrphanID { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string FullName { get; set; }

        public string Gender { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; set; }

        public string LCMStatus { get; set; }

        public string ProfileNumber { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Entry Date")]
        public DateTime EntryDate { get; set; }

        // Many to One
        public Guardian Guardian { get; set; }
        public int? GuardianID { get; set; }

        // One to Many
        public List<Narration> Narrations { get; set; }

        public List<Academic> Academics { get; set; }

        // Calculated
        public int Age
        {
            get
            {
                return (int)Math.Floor((DateTime.Now - this.DateOfBirth).TotalDays / 365.242199);
            }
        }
    }
}
