﻿// <auto-generated />
using System;
using System.Collections.Generic;
using DotNet2020.Domain._3.Models.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace DotNet2020.Data.Migrations.Attestation
{
    [DbContext(typeof(AttestationContext))]
    [Migration("20200323234729_AddAttestation")]
    partial class AddAttestation
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

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

                    b.Property<List<long>>("CompetencesId")
                        .HasColumnType("bigint[]");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Feedback")
                        .HasColumnType("text");

                    b.Property<string>("NextMoves")
                        .HasColumnType("text");

                    b.Property<string>("Problems")
                        .HasColumnType("text");

                    b.Property<long?>("WorkerId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("Attestations");
                });

            modelBuilder.Entity("DotNet2020.Domain._3.Models.CompetencesModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Competence")
                        .HasColumnType("text");

                    b.Property<string[]>("Content")
                        .HasColumnType("text[]");

                    b.Property<string[]>("Questions")
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

            modelBuilder.Entity("DotNet2020.Domain._3.Models.SpecificWorkerCompetencesModel", b =>
                {
                    b.Property<long>("WorkerId")
                        .HasColumnType("bigint");

                    b.Property<long>("CompetenceId")
                        .HasColumnType("bigint");

                    b.HasKey("WorkerId", "CompetenceId");

                    b.HasIndex("CompetenceId");

                    b.ToTable("SpecificWorkerCompetences");
                });

            modelBuilder.Entity("DotNet2020.Domain._3.Models.SpecificWorkerModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<double>("Bonus")
                        .HasColumnType("double precision");

                    b.Property<string>("Commentary")
                        .HasColumnType("text");

                    b.Property<string>("Experience")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Position")
                        .HasColumnType("text");

                    b.Property<string>("PreviousWorkPlaces")
                        .HasColumnType("text");

                    b.Property<double>("Salary")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.ToTable("Workers");
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
#pragma warning restore 612, 618
        }
    }
}