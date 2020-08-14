using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMSDesktopUWP.Core.HttpRepository
{
    public static class HttpRepositorySettings
    {
        // my local http: "http://localhost:51493/api";
        // my local https: https://localhost:44352/api
        // azure: https://lcmsmswebapi20200711192512.azurewebsites.net/api
        public static string BaseApiUrl { get; } = "https://lcmsmswebapi20200711192512.azurewebsites.net/api";
    }
}