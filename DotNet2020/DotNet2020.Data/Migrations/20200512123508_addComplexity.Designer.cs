﻿// <auto-generated />
using System;
using System.Collections.Generic;
using DotNet2020.Data;
using DotNet2020.Domain._4.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace DotNet2020.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20200512123508_addComplexity")]
    partial class addComplexity
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("DotNet2020.Data.AppIdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("character varying(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasColumnType("character varying(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("DotNet2020.Data.AppIdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasColumnType("character varying(256)")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<int?>("EmployeeId")
                        .HasColumnType("integer");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("character varying(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("character varying(256)")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .HasColumnType("character varying(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("DotNet2020.Domain.Core.Models.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("character varying(255)")
                        .HasMaxLength(255);

                    b.Property<string>("Group")
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("character varying(255)")
                        .HasMaxLength(255);

                    b.Property<string>("MiddleName")
                        .HasColumnType("character varying(255)")
                        .HasMaxLength(255);

                    b.Property<int?>("PositionId")
                        .HasColumnType("integer");

                    b.Property<string>("Type")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("PositionId");

                    b.ToTable("Employee");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Employee");
                });

            modelBuilder.Entity("DotNet2020.Domain.Core.Models.Position", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("character varying(255)")
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.ToTable("Position");
                });

            modelBuilder.Entity("DotNet2020.Domain.Models.EmployeeCalendar", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int?>("EmployeeId")
                        .HasColumnType("integer");

                    b.Property<bool>("IsLastVacationApproved")
                        .HasColumnType("boolean");

                    b.Property<int>("TotalDayOfVacation")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.ToTable("EmployeeCalendar");
                });

            modelBuilder.Entity("DotNet2020.Domain._3.Models.AnswerModel", b =>
                {
                    b.Property<long>("AnswerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Commentary")
                        .HasColumnType("text");

                    b.Property<bool>("IsRight")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsSkipped")
                        .HasColumnType("boolean");

                    b.Property<int>("NumberOfAsk")
                        .HasColumnType("integer");

                    b.Property<string>("Question")
                        .HasColumnType("text");

                    b.HasKey("AnswerId");

                    b.ToTable("Answers");
                });

            modelBuilder.Entity("DotNet2020.Domain._3.Models.AttestationAnswerModel", b =>
                {
                    b.Property<long>("AttestationId")
                        .HasColumnType("bigint");

                    b.Property<long>("AnswerId")
                        .HasColumnType("bigint");

                    b.HasKey("AttestationId", "AnswerId");

                    b.HasIndex("AnswerId");

                    b.ToTable("AttestationAnswer");
                });

            modelBuilder.Entity("DotNet2020.Domain._3.Models.AttestationModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Feedback")
                        .HasColumnType("text");

                    b.Property<List<long>>("GotCompetences")
                        .HasColumnType("bigint[]");

                    b.Property<List<long>>("IdsTestedCompetences")
                        .HasColumnType("bigint[]");

                    b.Property<string>("NextMoves")
                        .HasColumnType("text");

                    b.Property<string>("Problems")
                        .HasColumnType("text");

                    b.Property<long>("WorkerId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("Attestations");
                });

            modelBuilder.Entity("DotNet2020.Domain._3.Models.CompetenceQuestionsModel", b =>
                {
                    b.Property<long>("CompetenceId")
                        .HasColumnType("bigint");

                    b.Property<long>("QuestionId")
                        .HasColumnType("bigint");

                    b.HasKey("CompetenceId", "QuestionId");

                    b.ToTable("CompetenceQuestions");
                });

            modelBuilder.Entity("DotNet2020.Domain._3.Models.CompetencesModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Competence")
                        .HasColumnType("text");

                    b.Property<List<string>>("Content")
                        .HasColumnType("text[]");

                    b.HasKey("Id");

                    b.ToTable("Competences");
                });

            modelBuilder.Entity("DotNet2020.Domain._3.Models.GradeCompetencesModel", b =>
                {
                    b.Property<long>("GradeId")
                        .HasColumnType("bigint");

                    b.Property<long>("CompetenceId")
                        .HasColumnType("bigint");

                    b.HasKey("GradeId", "CompetenceId");

                    b.HasIndex("CompetenceId");

                    b.ToTable("GradeCompetences");
                });

            modelBuilder.Entity("DotNet2020.Domain._3.Models.GradeToGradeModel", b =>
                {
                    b.Property<long>("GradeId")
                        .HasColumnType("bigint");

                    b.Property<long>("NextGradeId")
                        .HasColumnType("bigint");

                    b.HasKey("GradeId", "NextGradeId");

                    b.ToTable("GradeToGrade");
                });

            modelBuilder.Entity("DotNet2020.Domain._3.Models.GradesModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Grade")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Grades");
                });

            modelBuilder.Entity("DotNet2020.Domain._3.Models.QuestionComplexityModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("QuestionComplexity");
                });

            modelBuilder.Entity("DotNet2020.Domain._3.Models.QuestionModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<long?>("CompetencesModelId")
                        .HasColumnType("bigint");

                    b.Property<int?>("ComplexityId")
                        .HasColumnType("integer");

                    b.Property<string>("Question")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CompetencesModelId");

                    b.HasIndex("ComplexityId");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("DotNet2020.Domain._3.Models.SpecificWorkerCompetencesModel", b =>
                {
                    b.Property<int>("WorkerId")
                        .HasColumnType("integer");

                    b.Property<long>("CompetenceId")
                        .HasColumnType("bigint");

                    b.HasKey("WorkerId", "CompetenceId");

                    b.HasIndex("CompetenceId");

                    b.ToTable("SpecificWorkerCompetences");
                });

            modelBuilder.Entity("DotNet2020.Domain._4.Models.AbstractCalendarEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("AbsenceType")
                        .HasColumnType("integer");

                    b.Property<int>("CalendarEmployeeId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("From")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("To")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("CalendarEmployeeId");

                    b.ToTable("AbstractCalendarEntries");

                    b.HasDiscriminator<int>("AbsenceType");
                });

            modelBuilder.Entity("DotNet2020.Domain._4.Models.Holiday", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Holidays");
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

            modelBuilder.Entity("DotNet2020.Domain._6.Models.FunctioningCapacityProject", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("FunctioningCapacity")
                        .HasColumnType("integer");

                    b.Property<int>("PeriodId")
                        .HasColumnType("integer");

                    b.Property<int>("ProjectId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("PeriodId");

                    b.HasIndex("ProjectId");

                    b.ToTable("FunctioningCapacityProjects");
                });

            modelBuilder.Entity("DotNet2020.Domain._6.Models.FunctioningCapacityResource", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("FunctionCapacity")
                        .HasColumnType("integer");

                    b.Property<int>("PeriodId")
                        .HasColumnType("integer");

                    b.Property<int>("ProjectId")
                        .HasColumnType("integer");

                    b.Property<int>("ResourceId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("PeriodId");

                    b.HasIndex("ProjectId");

                    b.HasIndex("ResourceId");

                    b.ToTable("FunctioningCapacityResources");
                });

            modelBuilder.Entity("DotNet2020.Domain._6.Models.Period", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("End")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("Start")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.ToTable("Periods");
                });

            modelBuilder.Entity("DotNet2020.Domain._6.Models.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int>("ProjectStatusId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ProjectStatusId");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("DotNet2020.Domain._6.Models.ProjectStatus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Status")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("ProjectStatuses");
                });

            modelBuilder.Entity("DotNet2020.Domain._6.Models.Resource", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("EmployeeId")
                        .HasColumnType("integer");

                    b.Property<int>("ResourceGroupTypeId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("ResourceGroupTypeId");

                    b.ToTable("Resources");
                });

            modelBuilder.Entity("DotNet2020.Domain._6.Models.ResourceCapacity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("Capacity")
                        .HasColumnType("integer");

                    b.Property<int>("PeriodId")
                        .HasColumnType("integer");

                    b.Property<int>("ResourceId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("PeriodId");

                    b.HasIndex("ResourceId");

                    b.ToTable("ResourceCapacities");
                });

            modelBuilder.Entity("DotNet2020.Domain._6.Models.ResourceGroupType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Group")
                        .HasColumnType("text");

                    b.Property<string>("Type")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("ResourceGroupsTypes");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .HasColumnType("text");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("DotNet2020.Domain._3.Models.SpecificWorkerModel", b =>
                {
                    b.HasBaseType("DotNet2020.Domain.Core.Models.Employee");

                    b.Property<double>("Bonus")
                        .HasColumnType("double precision");

                    b.Property<string>("Commentary")
                        .HasColumnType("text");

                    b.Property<string>("Experience")
                        .HasColumnType("text");

                    b.Property<string>("PreviousWorkPlaces")
                        .HasColumnType("text");

                    b.Property<double>("Salary")
                        .HasColumnType("double precision");

                    b.HasDiscriminator().HasValue("SpecificWorkerModel");
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

            modelBuilder.Entity("DotNet2020.Data.AppIdentityUser", b =>
                {
                    b.HasOne("DotNet2020.Domain.Core.Models.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeId");
                });

            modelBuilder.Entity("DotNet2020.Domain.Core.Models.Employee", b =>
                {
                    b.HasOne("DotNet2020.Domain.Core.Models.Position", "Position")
                        .WithMany()
                        .HasForeignKey("PositionId");
                });

            modelBuilder.Entity("DotNet2020.Domain.Models.EmployeeCalendar", b =>
                {
                    b.HasOne("DotNet2020.Domain.Core.Models.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeId");
                });

            modelBuilder.Entity("DotNet2020.Domain._3.Models.AttestationAnswerModel", b =>
                {
                    b.HasOne("DotNet2020.Domain._3.Models.AnswerModel", "Answer")
                        .WithMany("AttestationAnswer")
                        .HasForeignKey("AnswerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DotNet2020.Domain._3.Models.AttestationModel", "Attestation")
                        .WithMany("AttestationAnswer")
                        .HasForeignKey("AttestationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DotNet2020.Domain._3.Models.GradeCompetencesModel", b =>
                {
                    b.HasOne("DotNet2020.Domain._3.Models.CompetencesModel", "Competence")
                        .WithMany("GradesCompetences")
                        .HasForeignKey("CompetenceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DotNet2020.Domain._3.Models.GradesModel", "Grade")
                        .WithMany("GradesCompetences")
                        .HasForeignKey("GradeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DotNet2020.Domain._3.Models.QuestionModel", b =>
                {
                    b.HasOne("DotNet2020.Domain._3.Models.CompetencesModel", null)
                        .WithMany("Questions")
                        .HasForeignKey("CompetencesModelId");

                    b.HasOne("DotNet2020.Domain._3.Models.QuestionComplexityModel", "Complexity")
                        .WithMany()
                        .HasForeignKey("ComplexityId");
                });

            modelBuilder.Entity("DotNet2020.Domain._3.Models.SpecificWorkerCompetencesModel", b =>
                {
                    b.HasOne("DotNet2020.Domain._3.Models.CompetencesModel", "Competence")
                        .WithMany("SpecificWorkerCompetencesModels")
                        .HasForeignKey("CompetenceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DotNet2020.Domain._3.Models.SpecificWorkerModel", "Worker")
                        .WithMany("SpecificWorkerCompetencesModels")
                        .HasForeignKey("WorkerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DotNet2020.Domain._4.Models.AbstractCalendarEntry", b =>
                {
                    b.HasOne("DotNet2020.Domain.Models.EmployeeCalendar", "CalendarEmployee")
                        .WithMany()
                        .HasForeignKey("CalendarEmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DotNet2020.Domain._6.Models.FunctioningCapacityProject", b =>
                {
                    b.HasOne("DotNet2020.Domain._6.Models.Period", "Period")
                        .WithMany()
                        .HasForeignKey("PeriodId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DotNet2020.Domain._6.Models.Project", "Project")
                        .WithMany()
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DotNet2020.Domain._6.Models.FunctioningCapacityResource", b =>
                {
                    b.HasOne("DotNet2020.Domain._6.Models.Period", "Period")
                        .WithMany()
                        .HasForeignKey("PeriodId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DotNet2020.Domain._6.Models.Project", "Project")
                        .WithMany()
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DotNet2020.Domain._6.Models.Resource", "Resource")
                        .WithMany()
                        .HasForeignKey("ResourceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DotNet2020.Domain._6.Models.Project", b =>
                {
                    b.HasOne("DotNet2020.Domain._6.Models.ProjectStatus", "ProjectStatus")
                        .WithMany()
                        .HasForeignKey("ProjectStatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DotNet2020.Domain._6.Models.Resource", b =>
                {
                    b.HasOne("DotNet2020.Domain.Core.Models.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DotNet2020.Domain._6.Models.ResourceGroupType", "ResourceGroupType")
                        .WithMany()
                        .HasForeignKey("ResourceGroupTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DotNet2020.Domain._6.Models.ResourceCapacity", b =>
                {
                    b.HasOne("DotNet2020.Domain._6.Models.Period", "Period")
                        .WithMany()
                        .HasForeignKey("PeriodId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DotNet2020.Domain._6.Models.Resource", "Resource")
                        .WithMany()
                        .HasForeignKey("ResourceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("DotNet2020.Data.AppIdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("DotNet2020.Data.AppIdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("DotNet2020.Data.AppIdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("DotNet2020.Data.AppIdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DotNet2020.Data.AppIdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("DotNet2020.Data.AppIdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
