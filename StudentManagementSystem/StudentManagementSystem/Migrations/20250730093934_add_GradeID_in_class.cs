using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class add_GradeID_in_class : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Students",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            // First add the column as nullable
            migrationBuilder.AddColumn<int>(
                name: "GradeId",
                table: "Classes",
                type: "int",
                nullable: true); // Changed to nullable

            // Update existing records to point to a valid Grade
            migrationBuilder.Sql("UPDATE Classes SET GradeId = (SELECT TOP 1 Id FROM Grades)");

            // Now alter the column to be non-nullable
            migrationBuilder.AlterColumn<int>(
                name: "GradeId",
                table: "Classes",
                type: "int",
                nullable: false);

            migrationBuilder.CreateIndex(
                name: "IX_Classes_GradeId",
                table: "Classes",
                column: "GradeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_Grades_GradeId",
                table: "Classes",
                column: "GradeId",
                principalTable: "Grades",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_Grades_GradeId",
                table: "Classes");

            migrationBuilder.DropIndex(
                name: "IX_Classes_GradeId",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "GradeId",
                table: "Classes");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}