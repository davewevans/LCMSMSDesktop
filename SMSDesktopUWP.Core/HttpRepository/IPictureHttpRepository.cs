using SMSDesktopUWP.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SMSDesktopUWP.Core.HttpRepository
{
    public interface IPictureHttpRepository
    {
        Task<string> UploadImageAsync(PictureCreation picCreation, Stream fileStream);
    }
}
