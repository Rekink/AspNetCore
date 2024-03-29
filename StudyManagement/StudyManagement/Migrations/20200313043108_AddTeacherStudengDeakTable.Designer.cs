﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StudyManagement.Model;

namespace StudyManagement.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20200313043108_AddTeacherStudengDeakTable")]
    partial class AddTeacherStudengDeakTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("StudyManagement.Model.Desk", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.Property<int?>("StudentId");

                    b.HasKey("Id");

                    b.HasIndex("StudentId");

                    b.ToTable("Desks");
                });

            modelBuilder.Entity("StudyManagement.Model.School", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Schools");
                });

            modelBuilder.Entity("StudyManagement.Model.Student", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("BirthDate");

                    b.Property<int?>("ClassName");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<int?>("Gender");

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<string>("PhotoPath");

                    b.HasKey("Id");

                    b.ToTable("Students");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            BirthDate = new DateTime(1991, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ClassName = 1,
                            Email = "rekink@yeah.net",
                            FirstName = "han",
                            Gender = 1,
                            LastName = "zhou"
                        },
                        new
                        {
                            Id = 2,
                            BirthDate = new DateTime(1991, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ClassName = 3,
                            Email = "rekinz@qq.com",
                            FirstName = "ke",
                            Gender = 1,
                            LastName = "zhou"
                        });
                });

            modelBuilder.Entity("StudyManagement.Model.Teacher", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.Property<int>("SchoolID");

                    b.HasKey("Id");

                    b.HasIndex("SchoolID");

                    b.ToTable("Teachers");
                });

            modelBuilder.Entity("StudyManagement.Model.Desk", b =>
                {
                    b.HasOne("StudyManagement.Model.Student", "Student")
                        .WithMany()
                        .HasForeignKey("StudentId");
                });

            modelBuilder.Entity("StudyManagement.Model.Teacher", b =>
                {
                    b.HasOne("StudyManagement.Model.School", "School")
                        .WithMany("Teachers")
                        .HasForeignKey("SchoolID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
