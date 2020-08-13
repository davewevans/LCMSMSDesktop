using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SMSDesktopUWP.Core.Models
{
    public class Guardian
    {
        [Key]
        public int GuardianID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Entry Date")]
        public DateTime EntryDate { get; set; }

        public string Location { get; set; }

        // One to Many
        public List<Orphan> Orphans { get; set; }

        public List<Narration> Narrations { get; set; }
    }
}
