using SMSDesktopUWP.Core.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace SMSDesktopUWP.Core.Services
{
    public static class AcademicDataService
    {
        private static string GetConnectionString()
        {
            // Attempt to get the connection string from a config file
            // Learn more about specifying the connection string in a config file at https://docs.microsoft.com/dotnet/api/system.configuration.configurationmanager?view=netframework-4.7.2
            var conStr = ConfigurationManager.ConnectionStrings["SMSCloudConnectionString"]?.ConnectionString;

            if (!string.IsNullOrWhiteSpace(conStr))
            {
                return conStr;
            }
            else
            {
                // If no connection string is specified in a config file, use this as a fallback.
                return @"Data Source=*server*\*instance*;Initial Catalog=*dbname*;Integrated Security=SSPI";
            }
        }

        public static async void AddAcademic(Academic inAcademic)
        {
            try
            {
                // Replace this with the API code.
                using (var context = new SMSContext())
                {
                    context.Academics.Add(inAcademic);
                    await context.SaveChangesAsync();
                }

            }
            catch (Exception eSql)
            {
                // Your code may benefit from more robust error handling or logging.
                // This logging is just a reminder that you should handle exceptions when connecting to remote data.
                System.Diagnostics.Debug.WriteLine($"Exception: {eSql.Message} {eSql.InnerException?.Message}");
            }
        }

        public static async void SaveAcademic(int orphanID, Academic inAcademic)
        {
            try
            {
                // Replace this with the API code.
                using (var context = new SMSContext())
                {

                    var updatedAcademic = context.Academics.Single(c => c.AcademicID == inAcademic.AcademicID);

                    updatedAcademic.AcademicID = inAcademic.AcademicID;
                    updatedAcademic.OrphanID = orphanID;
                    updatedAcademic.EntryDate = inAcademic.EntryDate;
                    updatedAcademic.Grade = inAcademic.Grade;
                    updatedAcademic.School = inAcademic.School;
                    updatedAcademic.KCPE = inAcademic.KCPE;
                    updatedAcademic.KCSE = inAcademic.KCSE;

                    context.Academics.Update(updatedAcademic);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception eSql)
            {
                // Your code may benefit from more robust error handling or logging.
                // This logging is just a reminder that you should handle exceptions when connecting to remote data.
                System.Diagnostics.Debug.WriteLine($"Exception: {eSql.Message} {eSql.InnerException?.Message}");
            }
        }

        public static async void DeleteAcademic(Academic inAcademic)
        {

            try
            {
                // Replace with API code
                using (var context = new SMSContext())
                {
                    context.Academics.Remove(inAcademic);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception eSql)
            {
                // Your code may benefit from more robust error handling or logging.
                // This logging is just a reminder that you should handle exceptions when connecting to remote data.
                System.Diagnostics.Debug.WriteLine($"Exception: {eSql.Message} {eSql.InnerException?.Message}");
            }

        }


    }
}
