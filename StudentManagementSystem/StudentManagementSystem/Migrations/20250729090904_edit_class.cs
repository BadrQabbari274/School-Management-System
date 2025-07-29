using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class edit_class : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Classes_ClassesId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_ClassesId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "ClassesId",
                table: "Students");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClassesId",
                table: "Students",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_ClassesId",
                table: "Students",
                column: "ClassesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Classes_ClassesId",
                table: "Students",
                column: "ClassesId",
                principalTable: "Classes",
                principalColumn: "Id");
        }
    }
}
