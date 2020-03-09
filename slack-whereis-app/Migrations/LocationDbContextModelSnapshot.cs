﻿// <auto-generated />
using HenryKam.SlackWhereIs.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace HenryKam.SlackWhereIs.Migrations
{
    [DbContext(typeof(LocationDbContext))]
    partial class LocationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("app")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("HenryKam.SlackWhereIs.Model.Location", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Description")
                        .HasColumnName("description")
                        .HasColumnType("text");

                    b.Property<string>("Floor")
                        .HasColumnName("floor")
                        .HasColumnType("text");

                    b.Property<string>("LocationImageUrl")
                        .HasColumnName("location_image_url")
                        .HasColumnType("text");

                    b.Property<string>("MapImageUrl")
                        .IsRequired()
                        .HasColumnName("map_image_url")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("name")
                        .HasColumnType("text");

                    b.Property<string>("Office")
                        .IsRequired()
                        .HasColumnName("office")
                        .HasColumnType("text");

                    b.Property<string>("type")
                        .IsRequired()
                        .HasColumnName("type")
                        .HasColumnType("text");

                    b.HasKey("Id")
                        .HasName("pk_location");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasName("ix_location_name");

                    b.ToTable("location");

                    b.HasDiscriminator<string>("type").HasValue("Location");
                });

            modelBuilder.Entity("HenryKam.SlackWhereIs.Model.MeetingRoom", b =>
                {
                    b.HasBaseType("HenryKam.SlackWhereIs.Model.Location");

                    b.Property<string>("EmailAddress")
                        .HasColumnName("email_address")
                        .HasColumnType("text");

                    b.HasDiscriminator().HasValue("MeetingRoom");
                });
#pragma warning restore 612, 618
        }
    }
}
