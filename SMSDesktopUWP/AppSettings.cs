using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSDesktopUWP
{
    public static class AppSettings
    {
        public static bool UseWebApi {
            get {
                var value = ConfigurationManager.AppSettings.Get("UseWebApi");
                return Convert.ToBoolean(value);
            }
        }
    }
}
