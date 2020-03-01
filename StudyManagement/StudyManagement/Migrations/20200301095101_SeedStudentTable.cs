using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StudyManagement.Migrations
{
    public partial class SeedStudentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "Id", "BirthDate", "ClassName", "Email", "FirstName", "Gender", "LastName" },
                values: new object[] { 1, new DateTime(1991, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "rekink@yeah.net", "han", 1, "zhou" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
