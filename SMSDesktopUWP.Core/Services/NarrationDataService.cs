using Microsoft.EntityFrameworkCore;
using SMSDesktopUWP.Core.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SMSDesktopUWP.Core.Services
{
    public static class NarrationDataService
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

        public static async void AddNarration(Narration inNarration)
        {
            try
            {
                // Replace this with the API code.
                using (var context = new SMSContext())
                {
                    context.Narrations.Add(inNarration);
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

        public static async void SaveNarration(int orphanID, Narration inNarration)
        {
            try
            {
                // Replace this with the API code.
                using (var context = new SMSContext())
                {

                    var updatedNarration = context.Narrations.Single(c => c.NarrationID == inNarration.NarrationID);

                    updatedNarration.NarrationID = inNarration.NarrationID;
                    updatedNarration.OrphanID = orphanID;
                    updatedNarration.EntryDate = inNarration.EntryDate;
                    updatedNarration.Subject = inNarration.Subject;
                    updatedNarration.Note = inNarration.Note;

                    context.Narrations.Update(updatedNarration);
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

        public static async void DeleteNarration(Narration inNarration)
        {

            try
            {
                // Replace with API code
                using (var context = new SMSContext())
                {
                    context.Narrations.Remove(inNarration);
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

        public static NarrationStatistics GetNarrationStatistics()
        {
            NarrationStatistics narrStats = new NarrationStatistics();

            List<Narration> narrList = new List<Narration>();

            try
            {
                // Replace with API code
                using (var context = new SMSContext())
                {

                    narrList = context.Narrations.ToList();

                    narrStats.TotalNarrationCount = narrList.Count();
                    narrStats.OrphanNarrationCount = (from x in narrList where x.OrphanID != 0 && x.OrphanID != null select x).Count();
                    narrStats.GuardianNarrationCount = (from x in narrList where x.GuardianID != 0 && x.GuardianID != null select x).Count();
                    narrStats.OrphanLast6MoCount = narrList.Where(x => x.OrphanID != 0 && x.OrphanID != null &&
                        DateTime.Compare(x.EntryDate, DateTime.Today.AddMonths(-6)) >= 0).Count();
                    narrStats.GuardianLast6MoCount = narrList.Where(x => x.GuardianID != 0 && x.GuardianID != null && 
                        DateTime.Compare(x.EntryDate, DateTime.Today.AddMonths(-6)) >= 0).Count();

                    narrStats.OrphanLastContact = narrList.Where(x => x.OrphanID != 0 && x.OrphanID != null).OrderByDescending(d => d.EntryDate).FirstOrDefault().EntryDate;
                    narrStats.GuardianLastContact = narrList.Where(x => x.OrphanID != 0 && x.OrphanID != null).OrderByDescending(d => d.EntryDate).FirstOrDefault().EntryDate;

                }
            }
            catch (Exception eSql)
            {
                // Your code may benefit from more robust error handling or logging.
                // This logging is just a reminder that you should handle exceptions when connecting to remote data.
                System.Diagnostics.Debug.WriteLine($"Exception: {eSql.Message} {eSql.InnerException?.Message}");
            }

            return narrStats;
        }

    }
}
