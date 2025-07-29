using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class remove_class : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_Departments_Department_Id",
                table: "Classes");

            migrationBuilder.DropIndex(
                name: "IX_Classes_Department_Id",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "Department_Id",
                table: "Classes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Department_Id",
                table: "Classes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Classes_Department_Id",
                table: "Classes",
                column: "Department_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_Departments_Department_Id",
                table: "Classes",
                column: "Department_Id",
                principalTable: "Departments",
                principalColumn: "Id");
        }
    }
}
