using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class taskss : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Competencies_Id",
                table: "Tasks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_Competencies_Id",
                table: "Tasks",
                column: "Competencies_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Competencies_Competencies_Id",
                table: "Tasks",
                column: "Competencies_Id",
                principalTable: "Competencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Competencies_Competencies_Id",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_Competencies_Id",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "Competencies_Id",
                table: "Tasks");
        }
    }
}
