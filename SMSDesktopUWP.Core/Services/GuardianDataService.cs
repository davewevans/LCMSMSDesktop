using Microsoft.EntityFrameworkCore;
using SMSDesktopUWP.Core.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSDesktopUWP.Core.Services
{
    public static class GuardianDataService
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

        public static async Task<IEnumerable<Guardian>> AllGuardians()
        {
            // Using a hard-coded SQL statement for now to make it simpler.  Will need to either use API (preferred)
            // or stored procedure

            var guardianList = new List<Guardian>();

            try
            {
                using (var context = new SMSContext())
                {
                    guardianList = await context.Guardians
                        .Include(orphan => orphan.Orphans)
                        .Include(narrations => narrations.Narrations)
                        //.Include(notes => notes.Narrations
                        //    .OrderByDescending(n => n.EntryDate))
                        .ToListAsync();
                }

                #region Old code
                //using (var conn = new SqlConnection(GetConnectionString()))
                //{
                //    await conn.OpenAsync();

                //    if (conn.State == System.Data.ConnectionState.Open)
                //    {
                //        using (var cmd = conn.CreateCommand())
                //        {
                //            cmd.CommandText = getGuardianQuery;

                //            using (var reader = await cmd.ExecuteReaderAsync())
                //            {
                //                while (await reader.ReadAsync())
                //                {
                //                    //Orphan Data
                //                    var guardianID = !reader.IsDBNull(0) ? reader.GetInt32(0) : 0;
                //                    var firstName = reader.GetString(1);
                //                    var lastName = reader.GetString(2);
                //                    var fullName = reader.GetString(3);
                //                    var entryDate = !reader.IsDBNull(4) ? reader.GetDateTime(4) : default(DateTime);
                //                    var location = reader.GetString(5);

                //                    Guardian inGuardian = new Guardian()
                //                    {
                //                        GuardianID = guardianID,
                //                        FirstName = firstName,
                //                        LastName = lastName,
                //                        FullName = fullName,
                //                        EntryDate = entryDate,
                //                        Location = location
                //                    };

                //                    // Add to the List<>
                //                    guardianList.Add(inGuardian);
                //                }
                //            }
                //        }
                //    }
                //}
                #endregion
            }
            catch (Exception eSql)
            {
                // Your code may benefit from more robust error handling or logging.
                // This logging is just a reminder that you should handle exceptions when connecting to remote data.
                System.Diagnostics.Debug.WriteLine($"Exception: {eSql.Message} {eSql.InnerException?.Message}");
            }

            return guardianList;
        }

        public static async void AddGuardian(Guardian inGuardian)
        {
            try
            {
                // Replace this with the API code.
                using (var context = new SMSContext())
                {
                    context.Guardians.Add(inGuardian);
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

        public static async void SaveGuardian(Guardian inGuardian)
        {
            try
            {
                // Replace this with the API code.
                using (var context = new SMSContext())
                {

                    var updatedGuardian = context.Guardians.Single(c => c.GuardianID == inGuardian.GuardianID);

                    updatedGuardian.GuardianID = inGuardian.GuardianID;
                    updatedGuardian.FirstName = inGuardian.FirstName;
                    updatedGuardian.LastName = inGuardian.LastName;
                    updatedGuardian.FullName = inGuardian.FullName;
                    updatedGuardian.EntryDate = inGuardian.EntryDate;
                    updatedGuardian.Location = inGuardian.Location;

                    context.Guardians.Update(updatedGuardian);
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

        public static async void DeleteGuardian(Guardian inGuardian)
        {
            //TODO: If you delete a Guardian that has an Orphan, you will need to make sure the Orphan's
            //      Guardian entry is nulled out.

            try
            {
                // Replace with API code
                using (var context = new SMSContext())
                {
                    context.Guardians.Remove(inGuardian);
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

        public static async Task<Guardian> GetGuardianByID(int id)
        {
            Guardian returnGuardian = new Guardian();

            try
            {
                using (var context = new SMSContext())
                {
                    returnGuardian = await context.Guardians.FirstAsync(c => c.GuardianID == id);
                }
            }
            catch (Exception eSql)
            {
                // Your code may benefit from more robust error handling or logging.
                // This logging is just a reminder that you should handle exceptions when connecting to remote data.
                System.Diagnostics.Debug.WriteLine($"Exception: {eSql.Message} {eSql.InnerException?.Message}");
            }

            return returnGuardian;
        }

        public static GuardianStatistics GetGuardianStatistics()
        {
            GuardianStatistics guardStats = new GuardianStatistics();

            List<Guardian> guardList = new List<Guardian>();

            try
            {
                // Replace with API code
                using (var context = new SMSContext())
                {

                    guardList = context.Guardians.ToList();

                    guardStats.TotalCount = guardList.Count();
                }
            }
            catch (Exception eSql)
            {
                // Your code may benefit from more robust error handling or logging.
                // This logging is just a reminder that you should handle exceptions when connecting to remote data.
                System.Diagnostics.Debug.WriteLine($"Exception: {eSql.Message} {eSql.InnerException?.Message}");
            }

            return guardStats;
        }

    }
}
