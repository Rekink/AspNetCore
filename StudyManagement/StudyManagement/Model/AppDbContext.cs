using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudyManagement.Model
{
    public class AppDbContext : DbContext
    {
        /// <summary>
        /// 将应用程序的配置传递给DbContext
        /// </summary>
        /// <param name="option"></param>
        public AppDbContext(DbContextOptions<AppDbContext> option) : base(option)
        {

        }

        // 对要使用到的每个实体都添加 DbSet<TEntity> 属性
        // 通过DbSet属性来进行增删改查操作
        // 对DbSet采用Linq查询的时候，EFCore自动将其转换为SQL语句
        public DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 
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
            base.OnModelCreating(modelBuilder);
        }


    }
}
