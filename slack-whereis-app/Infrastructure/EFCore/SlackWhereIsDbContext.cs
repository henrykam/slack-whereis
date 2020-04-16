using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HenryKam.SlackWhereIs.Infrastructure.EFCore.Model;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Npgsql;

namespace HenryKam.SlackWhereIs.Infrastructure.EFCore
{
    public class SlackWhereIsDbContext : DbContext
    {


        public SlackWhereIsDbContext(DbContextOptions<SlackWhereIsDbContext> options) : base(options)
        {
        }

        public DbSet<Location> Location { get; set; }
        public DbSet<MeetingRoom> MeetingRoom { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<Tag> Tag { get; set; }

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
            modelBuilder.Entity<Location>(entity =>
            {
                entity.ToTable("location").HasKey(k => k.Id);
                entity.HasIndex(i => i.Name).IsUnique();
                entity.HasDiscriminator<string>("type");
                //entity.Property(p => p.LocationTags).HasConversion(
                //    o => JsonConvert.SerializeObject(o.Select(s=>s.Tag.Value)),
                //    v => JsonConvert.DeserializeObject<IEnumerable<string>>(v)
                //        .Select(s=>new LocationTag() { Tag = new Tag() { Value = v } }));
                //entity.HasIndex(i => i.Tags);
                //entity.Property(p => p.Tags).HasConversion(
                //    v => JsonConvert.SerializeObject(v),
                //    v => JsonConvert.DeserializeObject<List<string>>(v));
                });

        
            modelBuilder.Entity<LocationTag>(entity =>
            {
                entity.ToTable("locationtag").HasKey(k => new { k.LocationId, k.TagId });
                entity.HasOne(lt => lt.Location)
                    .WithMany(l => l.LocationTags)
                    .HasForeignKey(lt => lt.LocationId);
                entity.HasOne(lt => lt.Tag)
                    .WithMany(t => t.LocationTags)
                    .HasForeignKey(lt => lt.TagId);
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.ToTable("tag").HasKey(k => k.Id);
                entity.HasIndex(i => i.Value).IsUnique();
            });


        }
    }
}
