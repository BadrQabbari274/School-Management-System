using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class last_edit_for_user : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LastEditBy_Id",
                table: "Employees",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_LastEditBy_Id",
                table: "Employees",
                column: "LastEditBy_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Employees_LastEditBy_Id",
                table: "Employees",
                column: "LastEditBy_Id",
                principalTable: "Employees",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Employees_LastEditBy_Id",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_LastEditBy_Id",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "LastEditBy_Id",
                table: "Employees");
        }
    }
}
