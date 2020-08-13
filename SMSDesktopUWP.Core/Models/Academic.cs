using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SMSDesktopUWP.Core.Models
{
    public class Academic
    {
        [Key]
        public int AcademicID { get; set; }

        public string Grade { get; set; }

        public string KCPE { get; set; }

        public string KCSE { get; set; }

        public string School { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Entry Date")]
        public DateTime EntryDate { get; set; }

        // Many to one
        public int OrphanID { get; set; }
    }
}
