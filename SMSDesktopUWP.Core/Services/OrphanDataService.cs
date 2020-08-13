using SMSDesktopUWP.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace SMSDesktopUWP.Core.Services
{
    public static class OrphanDataService
    {
        #region oldCode
        //public static IEnumerable<Orphan> AllOrphans()
        //{
        //    // List orders from all companies
        //    var orphans = (IEnumerable<Orphan>)GetAllOrphans();
        //    return orphans;
        //}

        //private static IEnumerable<Orphan> GetAllOrphans()
        //{
        //    using (SMSContext inContext = new SMSContext())
        //    {
        //        var inOrphanRecs = (from r in inContext.Orphans select r);

        //        return inOrphanRecs;
        //    }
        //}

        //public static async Task<IEnumerable<Orphan>> GetMasterDetailDataAsync()
        //{
        //    await Task.CompletedTask;
        //    return AllOrphans();
        //}
        #endregion oldCode

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

        public static async Task<IEnumerable<Orphan>> AllOrphans()
        {
            // Using a hard-coded SQL statement for now to make it simpler.  Will need to either use API (preferred)
            // or stored procedure

            //const string getOrphanQuery = @"SELECT * FROM dbo.Orphans";

            List<Orphan> orphanList = new List<Orphan>();

            try
            {
                using (var context = new SMSContext())
                {
                    orphanList = await context.Orphans
                        .Include(guardian => guardian.Guardian)
                        .Include(narrations => narrations.Narrations)
                        //.Include(notes => notes.Narrations
                        //    .OrderByDescending(n => n.EntryDate))
                        .Include(acad => acad.Academics)
                        .ToListAsync();
                }
            }
            catch (Exception eSql)
            {
                // Your code may benefit from more robust error handling or logging.
                // This logging is just a reminder that you should handle exceptions when connecting to remote data.
                System.Diagnostics.Debug.WriteLine($"Exception: {eSql.Message} {eSql.InnerException?.Message}");
            }

            return orphanList;

        }

        public static async void AddOrphan(Orphan inOrphan)
        {
            try
            {
                // Replace this with the API code.
                using (var context = new SMSContext())
                {
                    context.Orphans.Add(inOrphan);
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

        public static async void SaveOrphan(Orphan inOrphan)
        {
            try
            {
                // Replace this with the API code.
                using (var context = new SMSContext())
                {

                    var updatedOrphan = context.Orphans.Single(c => c.OrphanID == inOrphan.OrphanID);

                    updatedOrphan.OrphanID = inOrphan.OrphanID;
                    updatedOrphan.FirstName = inOrphan.FirstName;
                    updatedOrphan.LastName = inOrphan.LastName;
                    updatedOrphan.MiddleName = inOrphan.MiddleName;
                    updatedOrphan.FullName = inOrphan.FullName;
                    if (inOrphan.GuardianID != 0)
                    {
                        updatedOrphan.GuardianID = inOrphan.GuardianID;
                    }
                    updatedOrphan.LCMStatus = inOrphan.LCMStatus;
                    updatedOrphan.ProfileNumber = inOrphan.ProfileNumber;
                    updatedOrphan.Gender = inOrphan.Gender;
                    updatedOrphan.EntryDate = inOrphan.EntryDate;
                    updatedOrphan.DateOfBirth = inOrphan.DateOfBirth;

                    context.Orphans.Update(updatedOrphan);
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

        public static async void DeleteOrphan(Orphan inOrphan)
        {
            try
            {
                // Replace with API code
                using (var context = new SMSContext())
                {
                    context.Orphans.Remove(inOrphan);
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

        public static OrphanStatistics GetOrphanStatistics()
        {
            OrphanStatistics orphStats = new OrphanStatistics();

            List<Orphan> orphanList = new List<Orphan>();

            try
            {
                // Replace with API code
                using (var context = new SMSContext())
                {

                    orphanList = context.Orphans
                        .ToList();

                    orphStats.TotalCount = orphanList.Count();
                    orphStats.ActiveCount = (from x in orphanList where x.LCMStatus == "Active" select x).Count();
                    orphStats.InactiveCount = (from x in orphanList where x.LCMStatus == "Inactive" select x).Count();
                    orphStats.UnknownCount = orphStats.TotalCount - (orphStats.ActiveCount + orphStats.InactiveCount);

                }
            }
            catch (Exception eSql)
            {
                // Your code may benefit from more robust error handling or logging.
                // This logging is just a reminder that you should handle exceptions when connecting to remote data.
                System.Diagnostics.Debug.WriteLine($"Exception: {eSql.Message} {eSql.InnerException?.Message}");
            }

            return orphStats;
        }

    }
}
