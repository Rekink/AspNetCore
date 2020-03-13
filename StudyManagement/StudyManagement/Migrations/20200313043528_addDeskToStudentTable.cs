using Microsoft.EntityFrameworkCore.Migrations;

namespace StudyManagement.Migrations
{
    public partial class addDeskToStudentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DeskId",
                table: "Students",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_DeskId",
                table: "Students",
                column: "DeskId");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Desks_DeskId",
                table: "Students",
                column: "DeskId",
                principalTable: "Desks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Desks_DeskId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_DeskId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "DeskId",
                table: "Students");
        }
    }
}
