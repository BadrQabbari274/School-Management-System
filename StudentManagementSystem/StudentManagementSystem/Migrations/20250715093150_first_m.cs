using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class first_m : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AttendanceTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendanceTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbsenceReasons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbsenceReasons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Classes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FieldId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Competences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: true),
                    FieldId = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Competences", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Outcomes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CompId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Outcomes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Outcomes_Competences_CompId",
                        column: x => x.CompId,
                        principalTable: "Competences",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Username = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastLogin = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_Employees_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeTypes_Employees_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Grades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Grades_Employees_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Employees",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClassId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Students_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Students_Employees_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Employees",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Fields",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GradeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fields", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Fields_Employees_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Fields_Grades_GradeId",
                        column: x => x.GradeId,
                        principalTable: "Grades",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StudentAbsents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentGrade = table.Column<int>(type: "int", nullable: true),
                    AttendanceType = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: true),
                    TeacherId = table.Column<int>(type: "int", nullable: true),
                    AbsenceReasonId = table.Column<int>(type: "int", nullable: true),
                    AttendanceTypeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentAbsents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentAbsents_AbsenceReasons_AbsenceReasonId",
                        column: x => x.AbsenceReasonId,
                        principalTable: "AbsenceReasons",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StudentAbsents_AttendanceTypes_AttendanceTypeId",
                        column: x => x.AttendanceTypeId,
                        principalTable: "AttendanceTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StudentAbsents_Employees_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StudentAbsents_Employees_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StudentAbsents_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StudentAttendances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    State = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentAttendances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentAttendances_Employees_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StudentAttendances_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StudentGrades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GradeId = table.Column<int>(type: "int", nullable: true),
                    StudentId = table.Column<int>(type: "int", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentGrades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentGrades_Grades_GradeId",
                        column: x => x.GradeId,
                        principalTable: "Grades",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StudentGrades_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TaskEvaluations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    OutcomeId = table.Column<int>(type: "int", nullable: true),
                    StudentId = table.Column<int>(type: "int", nullable: true)
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
                name: "FieldEmployees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    FieldId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldEmployees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FieldEmployees_Employees_UserId",
                        column: x => x.UserId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FieldEmployees_Fields_FieldId",
                        column: x => x.FieldId,
                        principalTable: "Fields",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RequestExits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Reason = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AttendanceId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestExits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestExits_Employees_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RequestExits_StudentAttendances_AttendanceId",
                        column: x => x.AttendanceId,
                        principalTable: "StudentAttendances",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Pictures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FilePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    TaskId = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pictures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pictures_Employees_CreatedBy",
                        column: x => x.CreatedBy,
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
                name: "IX_AbsenceReasons_CreatedBy",
                table: "AbsenceReasons",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_CreatedBy",
                table: "Classes",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_FieldId",
                table: "Classes",
                column: "FieldId");

            migrationBuilder.CreateIndex(
                name: "IX_Competences_CreatedBy",
                table: "Competences",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Competences_FieldId",
                table: "Competences",
                column: "FieldId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_CreatedBy",
                table: "Employees",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_RoleId",
                table: "Employees",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeTypes_CreatedBy",
                table: "EmployeeTypes",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_FieldEmployees_FieldId",
                table: "FieldEmployees",
                column: "FieldId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldEmployees_UserId",
                table: "FieldEmployees",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Fields_CreatedBy",
                table: "Fields",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Fields_GradeId",
                table: "Fields",
                column: "GradeId");

            migrationBuilder.CreateIndex(
                name: "IX_Grades_CreatedBy",
                table: "Grades",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Outcomes_CompId",
                table: "Outcomes",
                column: "CompId");

            migrationBuilder.CreateIndex(
                name: "IX_Pictures_CreatedBy",
                table: "Pictures",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Pictures_StudentId",
                table: "Pictures",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Pictures_TaskId",
                table: "Pictures",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestExits_AttendanceId",
                table: "RequestExits",
                column: "AttendanceId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestExits_CreatedBy",
                table: "RequestExits",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAbsents_AbsenceReasonId",
                table: "StudentAbsents",
                column: "AbsenceReasonId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAbsents_AttendanceTypeId",
                table: "StudentAbsents",
                column: "AttendanceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAbsents_CreatedBy",
                table: "StudentAbsents",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAbsents_StudentId",
                table: "StudentAbsents",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAbsents_TeacherId",
                table: "StudentAbsents",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAttendances_CreatedBy",
                table: "StudentAttendances",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAttendances_StudentId",
                table: "StudentAttendances",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentGrades_GradeId",
                table: "StudentGrades",
                column: "GradeId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentGrades_StudentId",
                table: "StudentGrades",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_ClassId",
                table: "Students",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_CreatedBy",
                table: "Students",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TaskEvaluations_OutcomeId",
                table: "TaskEvaluations",
                column: "OutcomeId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskEvaluations_StudentId",
                table: "TaskEvaluations",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_AbsenceReasons_Employees_CreatedBy",
                table: "AbsenceReasons",
                column: "CreatedBy",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_Employees_CreatedBy",
                table: "Classes",
                column: "CreatedBy",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_Fields_FieldId",
                table: "Classes",
                column: "FieldId",
                principalTable: "Fields",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Competences_Employees_CreatedBy",
                table: "Competences",
                column: "CreatedBy",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Competences_Fields_FieldId",
                table: "Competences",
                column: "FieldId",
                principalTable: "Fields",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_EmployeeTypes_RoleId",
                table: "Employees",
                column: "RoleId",
                principalTable: "EmployeeTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeTypes_Employees_CreatedBy",
                table: "EmployeeTypes");

            migrationBuilder.DropTable(
                name: "FieldEmployees");

            migrationBuilder.DropTable(
                name: "Pictures");

            migrationBuilder.DropTable(
                name: "RequestExits");

            migrationBuilder.DropTable(
                name: "StudentAbsents");

            migrationBuilder.DropTable(
                name: "StudentGrades");

            migrationBuilder.DropTable(
                name: "TaskEvaluations");

            migrationBuilder.DropTable(
                name: "StudentAttendances");

            migrationBuilder.DropTable(
                name: "AbsenceReasons");

            migrationBuilder.DropTable(
                name: "AttendanceTypes");

            migrationBuilder.DropTable(
                name: "Outcomes");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Competences");

            migrationBuilder.DropTable(
                name: "Classes");

            migrationBuilder.DropTable(
                name: "Fields");

            migrationBuilder.DropTable(
                name: "Grades");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "EmployeeTypes");
        }
    }
}
