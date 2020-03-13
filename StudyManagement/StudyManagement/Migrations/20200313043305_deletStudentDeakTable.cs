using Microsoft.EntityFrameworkCore.Migrations;

namespace StudyManagement.Migrations
{
    public partial class deletStudentDeakTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Desks_Students_StudentId",
                table: "Desks");

            migrationBuilder.DropIndex(
                name: "IX_Desks_StudentId",
                table: "Desks");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "Desks");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StudentId",
                table: "Desks",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Desks_StudentId",
                table: "Desks",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Desks_Students_StudentId",
                table: "Desks",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
