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
    [Migration("20200301095748_AlterStudentsSeedData")]
    partial class AlterStudentsSeedData
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

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
#pragma warning restore 612, 618
        }
    }
}
