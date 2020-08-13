using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SMSDesktopUWP.Core.Models
{
    public class OrphanPicture
    {
        [Key]
        public int OrphanPictureID { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Entry Date")]
        public DateTime EntryDate { get; set; }

        // Many to One
        public int OrphanID { get; set; }

        public int PictureID { get; set; }
    }
}
