using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SMSDesktopUWP.Core.Models
{
    public class OrphanSponsor
    {
        [Key]
        public int OrphanSponsorID { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Entry Date")]
        public DateTime EntryDate { get; set; }

        // Many to One
        public int OrphanID { get; set; }

        public int SponsorID { get; set; }
    }
}
