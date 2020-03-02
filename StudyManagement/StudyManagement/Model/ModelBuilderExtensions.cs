using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudyManagement.Model
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().HasData(
                new Student
                {
                    Id = 1,
                    FirstName = "han",
                    LastName = "zhou",
                    Gender = Gender.男,
                    BirthDate = new DateTime(1991, 5, 10),
                    ClassName = ClassNameEnum.FirstGrade,
                    Email = "rekink@yeah.net"
                },
                new Student
                {
                    Id = 2,
                    FirstName = "ke",
                    LastName = "zhou",
                    Gender = Gender.男,
                    BirthDate = new DateTime(1991, 5, 10),
                    ClassName = ClassNameEnum.ThirdGrade,
                    Email = "rekinz@qq.com"
                });
        }
    }
}
