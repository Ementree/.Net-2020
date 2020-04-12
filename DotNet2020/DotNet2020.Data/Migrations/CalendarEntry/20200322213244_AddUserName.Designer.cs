﻿// <auto-generated />
using System;
using DotNet2020.Domain._4.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace DotNet2020.Data.Migrations.CalendarEntry
{
    [DbContext(typeof(CalendarEntryContext))]
    [Migration("20200322213244_AddUserName")]
    partial class AddUserName
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("DotNet2020.Domain._4.Models.AbstractCalendarEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("AbsenceType")
                        .HasColumnType("integer");

                    b.Property<DateTime>("From")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("To")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("UserName")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("CalendarEntries");

                    b.HasDiscriminator<int>("AbsenceType");
                });

            modelBuilder.Entity("DotNet2020.Domain._4.Models.Recommendation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("RecommendationText")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Recommendations");
                });

            modelBuilder.Entity("DotNet2020.Domain._4.Models.Illness", b =>
                {
                    b.HasBaseType("DotNet2020.Domain._4.Models.AbstractCalendarEntry");

                    b.Property<bool>("IsApproved")
                        .HasColumnType("boolean");

                    b.HasDiscriminator().HasValue(1);
                });

            modelBuilder.Entity("DotNet2020.Domain._4.Models.SickDay", b =>
                {
                    b.HasBaseType("DotNet2020.Domain._4.Models.AbstractCalendarEntry");

                    b.HasDiscriminator().HasValue(0);
                });

            modelBuilder.Entity("DotNet2020.Domain._4.Models.Vacation", b =>
                {
                    b.HasBaseType("DotNet2020.Domain._4.Models.AbstractCalendarEntry");

                    b.Property<bool>("IsApproved")
                        .HasColumnName("Vacation_IsApproved")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsPaid")
                        .HasColumnType("boolean");

                    b.HasDiscriminator().HasValue(2);
                });
#pragma warning restore 612, 618
        }
    }
}
