using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMSDesktopUWP.Core.Models
{
    public class UpdateProfilePicture
    {
        public string PictureFileName { get; set; }

        public string BaseUri { get; set; }

        public string Caption { get; set; }

        public int OrphanID { get; set; }
    }
}
