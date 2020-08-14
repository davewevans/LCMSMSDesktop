using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Threading.Tasks;
//using LCMSMSWebApi.Validations;
//using Microsoft.AspNetCore.Http;

namespace SMSDesktopUWP.Core.Models
{
    public class PictureCreation
    {
        public string PictureFileName { get; set; }

        public bool SetAsProfilePic { get; set; }

        public string Caption { get; set; }

        public int OrphanID { get; set; }
    }
}
