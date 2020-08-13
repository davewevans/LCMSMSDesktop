using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SMSDesktopUWP.Core.Models
{
    public class Narration
    {
        [Key]
        public int NarrationID { get; set; }

        public string Subject { get; set; }

        public string Note { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Entry Date")]
        public DateTime EntryDate { get; set; }

        //public bool EscalateNeed { get; set; }

        // Many to One
        public int? OrphanID { get; set; }

        public int? GuardianID { get; set; }

    }
}
