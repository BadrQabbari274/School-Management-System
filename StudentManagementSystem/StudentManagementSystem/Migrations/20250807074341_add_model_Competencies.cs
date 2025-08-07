using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class add_model_Competencies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Outcomes_Competences_CompId",
                table: "Outcomes");

            migrationBuilder.DropTable(
                name: "Competences");

            migrationBuilder.DropTable(
                name: "Pictures");

            migrationBuilder.DropTable(
                name: "TaskEvaluations");

            migrationBuilder.RenameColumn(
                name: "CompId",
                table: "Outcomes",
                newName: "CreatedBy_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Outcomes_CompId",
                table: "Outcomes",
                newName: "IX_Outcomes_CreatedBy_Id");

            migrationBuilder.AddColumn<int>(
                name: "Competency_Id",
                table: "Outcomes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Outcomes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "Competencies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Duration = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Section_Id = table.Column<int>(type: "int", nullable: false),
                    Max_Outcome = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy_Id = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Competencies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Competencies_Employees_CreatedBy_Id",
                        column: x => x.CreatedBy_Id,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Competencies_Sections_Section_Id",
                        column: x => x.Section_Id,
                        principalTable: "Sections",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Evidences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ispractical = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Outcome_Id = table.Column<int>(type: "int", nullable: false),
                    CreatedBy_Id = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Evidences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Evidences_Employees_CreatedBy_Id",
                        column: x => x.CreatedBy_Id,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Evidences_Outcomes_Outcome_Id",
                        column: x => x.Outcome_Id,
                        principalTable: "Outcomes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Try",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy_Id = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Try", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Try_Employees_CreatedBy_Id",
                        column: x => x.CreatedBy_Id,
                        principalTable: "Employees",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Student_Evidence",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Evidence_Id = table.Column<int>(type: "int", nullable: false),
                    Image_Path = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Try_Id = table.Column<int>(type: "int", nullable: false),
                    CreatedBy_Id = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Class_Id = table.Column<int>(type: "int", nullable: false),
                    Student_Id = table.Column<int>(type: "int", nullable: false),
                    Working_Year_Id = table.Column<int>(type: "int", nullable: false),
                    Section_id = table.Column<int>(type: "int", nullable: false)
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
                name: "IX_Outcomes_Competency_Id",
                table: "Outcomes",
                column: "Competency_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Competencies_CreatedBy_Id",
                table: "Competencies",
                column: "CreatedBy_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Competencies_Section_Id",
                table: "Competencies",
                column: "Section_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Evidences_CreatedBy_Id",
                table: "Evidences",
                column: "CreatedBy_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Evidences_Outcome_Id",
                table: "Evidences",
                column: "Outcome_Id");

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

            migrationBuilder.CreateIndex(
                name: "IX_Try_CreatedBy_Id",
                table: "Try",
                column: "CreatedBy_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Outcomes_Competencies_Competency_Id",
                table: "Outcomes",
                column: "Competency_Id",
                principalTable: "Competencies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Outcomes_Employees_CreatedBy_Id",
                table: "Outcomes",
                column: "CreatedBy_Id",
                principalTable: "Employees",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Outcomes_Competencies_Competency_Id",
                table: "Outcomes");

            migrationBuilder.DropForeignKey(
                name: "FK_Outcomes_Employees_CreatedBy_Id",
                table: "Outcomes");

            migrationBuilder.DropTable(
                name: "Competencies");

            migrationBuilder.DropTable(
                name: "Student_Evidence");

            migrationBuilder.DropTable(
                name: "Evidences");

            migrationBuilder.DropTable(
                name: "Try");

            migrationBuilder.DropIndex(
                name: "IX_Outcomes_Competency_Id",
                table: "Outcomes");

            migrationBuilder.DropColumn(
                name: "Competency_Id",
                table: "Outcomes");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Outcomes");

            migrationBuilder.RenameColumn(
                name: "CreatedBy_Id",
                table: "Outcomes",
                newName: "CompId");

            migrationBuilder.RenameIndex(
                name: "IX_Outcomes_CreatedBy_Id",
                table: "Outcomes",
                newName: "IX_Outcomes_CompId");

            migrationBuilder.CreateTable(
                name: "Competences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy_Id = table.Column<int>(type: "int", nullable: false),
                    Department_Id = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Competences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Competences_Departments_Department_Id",
                        column: x => x.Department_Id,
                        principalTable: "Departments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Competences_Employees_CreatedBy_Id",
                        column: x => x.CreatedBy_Id,
                        principalTable: "Employees",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TaskEvaluations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OutcomeId = table.Column<int>(type: "int", nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskEvaluations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskEvaluations_Outcomes_OutcomeId",
                        column: x => x.OutcomeId,
                        principalTable: "Outcomes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TaskEvaluations_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Pictures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy_Id = table.Column<int>(type: "int", nullable: true),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    TaskId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pictures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pictures_Employees_CreatedBy_Id",
                        column: x => x.CreatedBy_Id,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Pictures_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Pictures_TaskEvaluations_TaskId",
                        column: x => x.TaskId,
                        principalTable: "TaskEvaluations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Competences_CreatedBy_Id",
                table: "Competences",
                column: "CreatedBy_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Competences_Department_Id",
                table: "Competences",
                column: "Department_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Pictures_CreatedBy_Id",
                table: "Pictures",
                column: "CreatedBy_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Pictures_StudentId",
                table: "Pictures",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Pictures_TaskId",
                table: "Pictures",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskEvaluations_OutcomeId",
                table: "TaskEvaluations",
                column: "OutcomeId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskEvaluations_StudentId",
                table: "TaskEvaluations",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Outcomes_Competences_CompId",
                table: "Outcomes",
                column: "CompId",
                principalTable: "Competences",
                principalColumn: "Id");
        }
    }
}
