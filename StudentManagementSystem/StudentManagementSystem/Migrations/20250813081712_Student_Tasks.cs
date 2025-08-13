using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class Student_Tasks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Student_Evidence");

            migrationBuilder.AddColumn<int>(
                name: "Task_Id",
                table: "Evidences",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy_Id = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tasks_Employees_CreatedBy_Id",
                        column: x => x.CreatedBy_Id,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Student_Tasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Task_Id = table.Column<int>(type: "int", nullable: false),
                    Image_Path = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Try_Id = table.Column<int>(type: "int", nullable: false),
                    CreatedBy_Id = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Class_Id = table.Column<int>(type: "int", nullable: false),
                    Student_Id = table.Column<int>(type: "int", nullable: false),
                    Working_Year_Id = table.Column<int>(type: "int", nullable: false),
                    Section_id = table.Column<int>(type: "int", nullable: false),
                    EvidenceId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Student_Tasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Student_Tasks_Classes_Class_Id",
                        column: x => x.Class_Id,
                        principalTable: "Classes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Student_Tasks_Employees_CreatedBy_Id",
                        column: x => x.CreatedBy_Id,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Student_Tasks_Evidences_EvidenceId",
                        column: x => x.EvidenceId,
                        principalTable: "Evidences",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Student_Tasks_Student_Class_Section_Years_Student_Id_Working_Year_Id_Section_id",
                        columns: x => new { x.Student_Id, x.Working_Year_Id, x.Section_id },
                        principalTable: "Student_Class_Section_Years",
                        principalColumns: new[] { "Student_Id", "Working_Year_Id", "Section_id" });
                    table.ForeignKey(
                        name: "FK_Student_Tasks_Tasks_Task_Id",
                        column: x => x.Task_Id,
                        principalTable: "Tasks",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Student_Tasks_Try_Try_Id",
                        column: x => x.Try_Id,
                        principalTable: "Try",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Evidences_Task_Id",
                table: "Evidences",
                column: "Task_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Student_Tasks_Class_Id",
                table: "Student_Tasks",
                column: "Class_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Student_Tasks_CreatedBy_Id",
                table: "Student_Tasks",
                column: "CreatedBy_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Student_Tasks_EvidenceId",
                table: "Student_Tasks",
                column: "EvidenceId");

            migrationBuilder.CreateIndex(
                name: "IX_Student_Tasks_Student_Id_Working_Year_Id_Section_id",
                table: "Student_Tasks",
                columns: new[] { "Student_Id", "Working_Year_Id", "Section_id" });

            migrationBuilder.CreateIndex(
                name: "IX_Student_Tasks_Task_Id",
                table: "Student_Tasks",
                column: "Task_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Student_Tasks_Try_Id",
                table: "Student_Tasks",
                column: "Try_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_CreatedBy_Id",
                table: "Tasks",
                column: "CreatedBy_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Evidences_Tasks_Task_Id",
                table: "Evidences",
                column: "Task_Id",
                principalTable: "Tasks",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Evidences_Tasks_Task_Id",
                table: "Evidences");

            migrationBuilder.DropTable(
                name: "Student_Tasks");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Evidences_Task_Id",
                table: "Evidences");

            migrationBuilder.DropColumn(
                name: "Task_Id",
                table: "Evidences");

            migrationBuilder.CreateTable(
                name: "Student_Evidence",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Class_Id = table.Column<int>(type: "int", nullable: false),
                    CreatedBy_Id = table.Column<int>(type: "int", nullable: false),
                    Evidence_Id = table.Column<int>(type: "int", nullable: false),
                    Student_Id = table.Column<int>(type: "int", nullable: false),
                    Try_Id = table.Column<int>(type: "int", nullable: false),
                    Working_Year_Id = table.Column<int>(type: "int", nullable: false),
                    Section_id = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Image_Path = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Student_Evidence", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Student_Evidence_Classes_Class_Id",
                        column: x => x.Class_Id,
                        principalTable: "Classes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Student_Evidence_Employees_CreatedBy_Id",
                        column: x => x.CreatedBy_Id,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Student_Evidence_Evidences_Evidence_Id",
                        column: x => x.Evidence_Id,
                        principalTable: "Evidences",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Student_Evidence_Student_Class_Section_Years_Student_Id_Working_Year_Id_Section_id",
                        columns: x => new { x.Student_Id, x.Working_Year_Id, x.Section_id },
                        principalTable: "Student_Class_Section_Years",
                        principalColumns: new[] { "Student_Id", "Working_Year_Id", "Section_id" });
                    table.ForeignKey(
                        name: "FK_Student_Evidence_Try_Try_Id",
                        column: x => x.Try_Id,
                        principalTable: "Try",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Student_Evidence_Class_Id",
                table: "Student_Evidence",
                column: "Class_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Student_Evidence_CreatedBy_Id",
                table: "Student_Evidence",
                column: "CreatedBy_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Student_Evidence_Evidence_Id",
                table: "Student_Evidence",
                column: "Evidence_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Student_Evidence_Student_Id_Working_Year_Id_Section_id",
                table: "Student_Evidence",
                columns: new[] { "Student_Id", "Working_Year_Id", "Section_id" });

            migrationBuilder.CreateIndex(
                name: "IX_Student_Evidence_Try_Id",
                table: "Student_Evidence",
                column: "Try_Id");
        }
    }
}
