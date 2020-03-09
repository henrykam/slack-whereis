using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HenryKam.SlackWhereIs.Model;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace HenryKam.SlackWhereIs.Infrastructure
{
    public class LocationDbContext : DbContext
    {


        public LocationDbContext(DbContextOptions<LocationDbContext> options) : base(options)
        {
        }

        public DbSet<Location> Location { get; set; }
        public DbSet<MeetingRoom> MeetingRoom { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //if (!optionsBuilder.IsConfigured)
            //{
            //    //optionsBuilder.UseNpgsql(_connectionString, o => o.SetPostgresVersion(new Version(9, 5)));

            //    IConfigurationRoot configuration = new ConfigurationBuilder()
            //   .SetBasePath(Directory.GetCurrentDirectory())
            //   .AddJsonFile(Constants.SettingsFilename)
            //   .Build();
            //    var connectionString = configuration.GetConnectionString("GeoDeviceFreezeDb");
            //    optionsBuilder.UseNpgsql(connectionString, o => o.SetPostgresVersion(new Version(9, 5)));
            //}
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("app");
            //modelBuilder.HasPostgresEnum<LocationType>();
            modelBuilder.Entity<Location>(entity =>
            {
                entity.ToTable("location").HasKey(k => k.Id);
                entity.HasIndex(i => i.Name).IsUnique();
                entity.HasDiscriminator<string>("type");
            });


        }
    }
}
