using Microsoft.EntityFrameworkCore;
using SMSDesktopUWP.Core.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace SMSDesktopUWP.Core.Services
{
    public class SMSContext : DbContext
    {
        public SMSContext()
        {
            Database.EnsureCreated();
        }

        public DbSet<Orphan> Orphans { get; set; }

        public DbSet<Guardian> Guardians { get; set; }

        public DbSet<Sponsor> Sponsors { get; set; }

        public DbSet<Academic> Academics { get; set; }

        public DbSet<Narration> Narrations { get; set; }

        public DbSet<OrphanPicture> OrphanPictures { get; set; }

        public DbSet<OrphanSponsor> OrphanSponsors { get; set; }

        public DbSet<Picture> Pictures { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string conn = GetConnectionString("SMSCloudConnectionString");
            //string conn = GetConnectionString("SMSLocalConnectionString");

            optionsBuilder.UseSqlServer(conn);

            //optionsBuilder.UseSqlServer(
            //    @"Server=tcp:lcmsmsserver.database.windows.net,1433;Initial Catalog=SMSDatabase;Persist Security Info=False;User ID=pbolden;Password=K3nya4ever;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

            //optionsBuilder.UseSqlServer(
            //    @"Data Source=(localdb)\MSSQLLocalDB; Initial Catalog=SMSDatabase;");
        }

        // TODO WTS: Specify the connection string in a config file or below.
        private static string GetConnectionString(string conStr)
        {
            // Attempt to get the connection string from a config file
            // Learn more about specifying the connection string in a config file at https://docs.microsoft.com/dotnet/api/system.configuration.configurationmanager?view=netframework-4.7.2
            var connectionString = ConfigurationManager.ConnectionStrings[conStr]?.ConnectionString;

            if (!string.IsNullOrWhiteSpace(conStr))
            {
                return connectionString;
            }
            else
            {
                // If no connection string is specified in a config file, use this as a fallback.
                return @"Data Source=*server*\*instance*;Initial Catalog=*dbname*;Integrated Security=SSPI";
            }
        }
    }
}
