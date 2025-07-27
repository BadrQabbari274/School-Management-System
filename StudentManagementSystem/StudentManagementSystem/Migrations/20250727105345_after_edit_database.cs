using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class after_edit_database : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AbsenceReasons_Employees_CreatedBy",
                table: "AbsenceReasons");

            migrationBuilder.DropForeignKey(
                name: "FK_Classes_Employees_CreatedBy",
                table: "Classes");

            migrationBuilder.DropForeignKey(
                name: "FK_Classes_Fields_FieldId",
                table: "Classes");

            migrationBuilder.DropForeignKey(
                name: "FK_Competences_Employees_CreatedBy",
                table: "Competences");

            migrationBuilder.DropForeignKey(
                name: "FK_Competences_Fields_FieldId",
                table: "Competences");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_EmployeeTypes_RoleId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Employees_CreatedBy",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeTypes_Employees_CreatedBy",
                table: "EmployeeTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_Grades_Employees_CreatedBy",
                table: "Grades");

            migrationBuilder.DropForeignKey(
                name: "FK_Pictures_Employees_CreatedBy",
                table: "Pictures");

            migrationBuilder.DropForeignKey(
                name: "FK_RequestExits_Employees_CreatedBy",
                table: "RequestExits");

            migrationBuilder.DropForeignKey(
                name: "FK_RequestExits_Employees_ProcessedBy",
                table: "RequestExits");

            migrationBuilder.DropForeignKey(
                name: "FK_RequestExits_Students_StudentId",
                table: "RequestExits");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentAbsents_Employees_CreatedBy",
                table: "StudentAbsents");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentAbsents_Employees_TeacherId",
                table: "StudentAbsents");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentAbsents_Students_StudentId",
                table: "StudentAbsents");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentAttendances_Employees_CreatedBy",
                table: "StudentAttendances");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentAttendances_Students_StudentId",
                table: "StudentAttendances");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_Classes_ClassId",
                table: "Students");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_Employees_CreatedBy",
                table: "Students");

            migrationBuilder.DropTable(
                name: "FieldEmployees");

            migrationBuilder.DropTable(
                name: "Fields");

            migrationBuilder.DropIndex(
                name: "IX_StudentAttendances_CreatedBy",
                table: "StudentAttendances");

            migrationBuilder.DropIndex(
                name: "IX_StudentAttendances_StudentId",
                table: "StudentAttendances");

            migrationBuilder.DropIndex(
                name: "IX_StudentAbsents_CreatedBy",
                table: "StudentAbsents");

            migrationBuilder.DropIndex(
                name: "IX_StudentAbsents_StudentId",
                table: "StudentAbsents");

            migrationBuilder.DropIndex(
                name: "IX_StudentAbsents_TeacherId",
                table: "StudentAbsents");

            migrationBuilder.DropIndex(
                name: "IX_RequestExits_CreatedBy",
                table: "RequestExits");

            migrationBuilder.DropIndex(
                name: "IX_RequestExits_ProcessedBy",
                table: "RequestExits");

            migrationBuilder.DropIndex(
                name: "IX_RequestExits_StudentId",
                table: "RequestExits");

            migrationBuilder.DropIndex(
                name: "IX_Grades_CreatedBy",
                table: "Grades");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeTypes_CreatedBy",
                table: "EmployeeTypes");

            migrationBuilder.DropIndex(
                name: "IX_Employees_CreatedBy",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Competences_CreatedBy",
                table: "Competences");

            migrationBuilder.DropIndex(
                name: "IX_Competences_FieldId",
                table: "Competences");

            migrationBuilder.DropIndex(
                name: "IX_Classes_CreatedBy",
                table: "Classes");

            migrationBuilder.DropIndex(
                name: "IX_Classes_FieldId",
                table: "Classes");

            migrationBuilder.DropIndex(
                name: "IX_AbsenceReasons_CreatedBy",
                table: "AbsenceReasons");

            migrationBuilder.DropColumn(
                name: "Governarate",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "StudentAttendances");

            migrationBuilder.DropColumn(
                name: "State",
                table: "StudentAttendances");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "StudentAttendances");

            migrationBuilder.DropColumn(
                name: "AttendanceType",
                table: "StudentAbsents");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "StudentAbsents");

            migrationBuilder.DropColumn(
                name: "IsFieldAttendance",
                table: "StudentAbsents");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "StudentAbsents");

            migrationBuilder.DropColumn(
                name: "StudentGrade",
                table: "StudentAbsents");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "StudentAbsents");

            migrationBuilder.DropColumn(
                name: "TeacherId",
                table: "StudentAbsents");

            migrationBuilder.DropColumn(
                name: "ActualReturnTime",
                table: "RequestExits");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "RequestExits");

            migrationBuilder.DropColumn(
                name: "ExpectedReturnTime",
                table: "RequestExits");

            migrationBuilder.DropColumn(
                name: "ProcessedBy",
                table: "RequestExits");

            migrationBuilder.DropColumn(
                name: "ProcessedDate",
                table: "RequestExits");

            migrationBuilder.DropColumn(
                name: "ProcessingNotes",
                table: "RequestExits");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "RequestExits");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Grades");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "EmployeeTypes");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Competences");

            migrationBuilder.DropColumn(
                name: "FieldId",
                table: "Competences");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "FieldId",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "IsForFieldAttendance",
                table: "AttendanceTypes");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "AbsenceReasons");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Students",
                newName: "ClassesId");

            migrationBuilder.RenameColumn(
                name: "ClassId",
                table: "Students",
                newName: "CreatedBy_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Students_CreatedBy",
                table: "Students",
                newName: "IX_Students_ClassesId");

            migrationBuilder.RenameIndex(
                name: "IX_Students_ClassId",
                table: "Students",
                newName: "IX_Students_CreatedBy_Id");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "RequestExits",
                newName: "CreatedBy_Id");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Pictures",
                newName: "CreatedBy_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Pictures_CreatedBy",
                table: "Pictures",
                newName: "IX_Pictures_CreatedBy_Id");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "TaskEvaluations",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<int>(
                name: "StudentId",
                table: "TaskEvaluations",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "OutcomeId",
                table: "TaskEvaluations",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "birth_Certificate",
                table: "Students",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Picture_Profile",
                table: "Students",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Phone_Number_Mother",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Phone_Number_Father",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Phone_Number",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Natrual_Id",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Jop_of_Mother",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Jop_of_Father",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Date_of_birth",
                table: "Students",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Adress",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "Governate",
                table: "Students",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "StudentId",
                table: "StudentGrades",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "GradeId",
                table: "StudentGrades",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Working_Year_Id",
                table: "StudentGrades",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "StudentAttendances",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AttendanceTypeId",
                table: "StudentAttendances",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy_Id",
                table: "StudentAttendances",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CustomReasonDetails",
                table: "StudentAttendances",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "StudentClassSectionYear_Class_Id",
                table: "StudentAttendances",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StudentClassSectionYear_Section_id",
                table: "StudentAttendances",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StudentClassSectionYear_Student_Id",
                table: "StudentAttendances",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StudentClassSectionYear_Working_Year_Id",
                table: "StudentAttendances",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "CustomReasonDetails",
                table: "StudentAbsents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AttendanceTypeId",
                table: "StudentAbsents",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AbsenceReasonId",
                table: "StudentAbsents",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy_Id",
                table: "StudentAbsents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StudentClassSectionYear_Class_Id",
                table: "StudentAbsents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StudentClassSectionYear_Section_id",
                table: "StudentAbsents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StudentClassSectionYear_Student_Id",
                table: "StudentAbsents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StudentClassSectionYear_Working_Year_Id",
                table: "StudentAbsents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Reason",
                table: "RequestExits",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<int>(
                name: "AttendanceId",
                table: "RequestExits",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TaskId",
                table: "Pictures",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "StudentId",
                table: "Pictures",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FilePath",
                table: "Pictures",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Outcomes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<int>(
                name: "CompId",
                table: "Outcomes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Grades",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy_Id",
                table: "Grades",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "EmployeeTypes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy_Id",
                table: "EmployeeTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<int>(
                name: "RoleId",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastLogin",
                table: "Employees",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Join_Date",
                table: "Employees",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy_Id",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Competences",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<int>(
                name: "Duration",
                table: "Competences",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy_Id",
                table: "Competences",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Department_Id",
                table: "Competences",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Classes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy_Id",
                table: "Classes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Department_Id",
                table: "Classes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AttendanceTypes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "AttendanceTypes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AbsenceReasons",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy_Id",
                table: "AbsenceReasons",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy_Id = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GradeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Departments_Employees_CreatedBy_Id",
                        column: x => x.CreatedBy_Id,
                        principalTable: "Employees",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Working_Years",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Start_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    End_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy_Id = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Working_Years", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Working_Years_Employees_CreatedBy_Id",
                        column: x => x.CreatedBy_Id,
                        principalTable: "Employees",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Employee_Departments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Department_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee_Departments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employee_Departments_Departments_Department_Id",
                        column: x => x.Department_Id,
                        principalTable: "Departments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Employee_Departments_Employees_UserId",
                        column: x => x.UserId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Sections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Department_Id = table.Column<int>(type: "int", nullable: false),
                    Name_Of_Section = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy_Id = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sections_Departments_Department_Id",
                        column: x => x.Department_Id,
                        principalTable: "Departments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Sections_Employees_CreatedBy_Id",
                        column: x => x.CreatedBy_Id,
                        principalTable: "Employees",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Student_Class_Section_Years",
                columns: table => new
                {
                    Student_Id = table.Column<int>(type: "int", nullable: false),
                    Class_Id = table.Column<int>(type: "int", nullable: false),
                    Working_Year_Id = table.Column<int>(type: "int", nullable: false),
                    Section_id = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy_Id = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Student_Class_Section_Years", x => new { x.Student_Id, x.Class_Id, x.Working_Year_Id, x.Section_id });
                    table.ForeignKey(
                        name: "FK_Student_Class_Section_Years_Classes_Class_Id",
                        column: x => x.Class_Id,
                        principalTable: "Classes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Student_Class_Section_Years_Employees_CreatedBy_Id",
                        column: x => x.CreatedBy_Id,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Student_Class_Section_Years_Sections_Section_id",
                        column: x => x.Section_id,
                        principalTable: "Sections",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Student_Class_Section_Years_Students_Student_Id",
                        column: x => x.Student_Id,
                        principalTable: "Students",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Student_Class_Section_Years_Working_Years_Working_Year_Id",
                        column: x => x.Working_Year_Id,
                        principalTable: "Working_Years",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentGrades_Working_Year_Id",
                table: "StudentGrades",
                column: "Working_Year_Id");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAttendances_AttendanceTypeId",
                table: "StudentAttendances",
                column: "AttendanceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAttendances_CreatedBy_Id",
                table: "StudentAttendances",
                column: "CreatedBy_Id");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAttendances_StudentClassSectionYear_Student_Id_StudentClassSectionYear_Class_Id_StudentClassSectionYear_Working_Year_~",
                table: "StudentAttendances",
                columns: new[] { "StudentClassSectionYear_Student_Id", "StudentClassSectionYear_Class_Id", "StudentClassSectionYear_Working_Year_Id", "StudentClassSectionYear_Section_id" });

            migrationBuilder.CreateIndex(
                name: "IX_StudentAbsents_CreatedBy_Id",
                table: "StudentAbsents",
                column: "CreatedBy_Id");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAbsents_StudentClassSectionYear_Student_Id_StudentClassSectionYear_Class_Id_StudentClassSectionYear_Working_Year_Id_S~",
                table: "StudentAbsents",
                columns: new[] { "StudentClassSectionYear_Student_Id", "StudentClassSectionYear_Class_Id", "StudentClassSectionYear_Working_Year_Id", "StudentClassSectionYear_Section_id" });

            migrationBuilder.CreateIndex(
                name: "IX_RequestExits_CreatedBy_Id",
                table: "RequestExits",
                column: "CreatedBy_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Grades_CreatedBy_Id",
                table: "Grades",
                column: "CreatedBy_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_CreatedBy_Id",
                table: "Employees",
                column: "CreatedBy_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Competences_CreatedBy_Id",
                table: "Competences",
                column: "CreatedBy_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Competences_Department_Id",
                table: "Competences",
                column: "Department_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_CreatedBy_Id",
                table: "Classes",
                column: "CreatedBy_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_Department_Id",
                table: "Classes",
                column: "Department_Id");

            migrationBuilder.CreateIndex(
                name: "IX_AbsenceReasons_CreatedBy_Id",
                table: "AbsenceReasons",
                column: "CreatedBy_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_CreatedBy_Id",
                table: "Departments",
                column: "CreatedBy_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_Departments_Department_Id",
                table: "Employee_Departments",
                column: "Department_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_Departments_UserId",
                table: "Employee_Departments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Sections_CreatedBy_Id",
                table: "Sections",
                column: "CreatedBy_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Sections_Department_Id",
                table: "Sections",
                column: "Department_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Student_Class_Section_Years_Class_Id",
                table: "Student_Class_Section_Years",
                column: "Class_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Student_Class_Section_Years_CreatedBy_Id",
                table: "Student_Class_Section_Years",
                column: "CreatedBy_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Student_Class_Section_Years_Section_id",
                table: "Student_Class_Section_Years",
                column: "Section_id");

            migrationBuilder.CreateIndex(
                name: "IX_Student_Class_Section_Years_Working_Year_Id",
                table: "Student_Class_Section_Years",
                column: "Working_Year_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Working_Years_CreatedBy_Id",
                table: "Working_Years",
                column: "CreatedBy_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AbsenceReasons_Employees_CreatedBy_Id",
                table: "AbsenceReasons",
                column: "CreatedBy_Id",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_Departments_Department_Id",
                table: "Classes",
                column: "Department_Id",
                principalTable: "Departments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_Employees_CreatedBy_Id",
                table: "Classes",
                column: "CreatedBy_Id",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Competences_Departments_Department_Id",
                table: "Competences",
                column: "Department_Id",
                principalTable: "Departments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Competences_Employees_CreatedBy_Id",
                table: "Competences",
                column: "CreatedBy_Id",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_EmployeeTypes_RoleId",
                table: "Employees",
                column: "RoleId",
                principalTable: "EmployeeTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Employees_CreatedBy_Id",
                table: "Employees",
                column: "CreatedBy_Id",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Grades_Employees_CreatedBy_Id",
                table: "Grades",
                column: "CreatedBy_Id",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Pictures_Employees_CreatedBy_Id",
                table: "Pictures",
                column: "CreatedBy_Id",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestExits_Employees_CreatedBy_Id",
                table: "RequestExits",
                column: "CreatedBy_Id",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAbsents_Employees_CreatedBy_Id",
                table: "StudentAbsents",
                column: "CreatedBy_Id",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAbsents_Student_Class_Section_Years_StudentClassSectionYear_Student_Id_StudentClassSectionYear_Class_Id_StudentClassS~",
                table: "StudentAbsents",
                columns: new[] { "StudentClassSectionYear_Student_Id", "StudentClassSectionYear_Class_Id", "StudentClassSectionYear_Working_Year_Id", "StudentClassSectionYear_Section_id" },
                principalTable: "Student_Class_Section_Years",
                principalColumns: new[] { "Student_Id", "Class_Id", "Working_Year_Id", "Section_id" });

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAttendances_AttendanceTypes_AttendanceTypeId",
                table: "StudentAttendances",
                column: "AttendanceTypeId",
                principalTable: "AttendanceTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAttendances_Employees_CreatedBy_Id",
                table: "StudentAttendances",
                column: "CreatedBy_Id",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAttendances_Student_Class_Section_Years_StudentClassSectionYear_Student_Id_StudentClassSectionYear_Class_Id_StudentCl~",
                table: "StudentAttendances",
                columns: new[] { "StudentClassSectionYear_Student_Id", "StudentClassSectionYear_Class_Id", "StudentClassSectionYear_Working_Year_Id", "StudentClassSectionYear_Section_id" },
                principalTable: "Student_Class_Section_Years",
                principalColumns: new[] { "Student_Id", "Class_Id", "Working_Year_Id", "Section_id" });

            migrationBuilder.AddForeignKey(
                name: "FK_StudentGrades_Working_Years_Working_Year_Id",
                table: "StudentGrades",
                column: "Working_Year_Id",
                principalTable: "Working_Years",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Classes_ClassesId",
                table: "Students",
                column: "ClassesId",
                principalTable: "Classes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Employees_CreatedBy_Id",
                table: "Students",
                column: "CreatedBy_Id",
                principalTable: "Employees",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AbsenceReasons_Employees_CreatedBy_Id",
                table: "AbsenceReasons");

            migrationBuilder.DropForeignKey(
                name: "FK_Classes_Departments_Department_Id",
                table: "Classes");

            migrationBuilder.DropForeignKey(
                name: "FK_Classes_Employees_CreatedBy_Id",
                table: "Classes");

            migrationBuilder.DropForeignKey(
                name: "FK_Competences_Departments_Department_Id",
                table: "Competences");

            migrationBuilder.DropForeignKey(
                name: "FK_Competences_Employees_CreatedBy_Id",
                table: "Competences");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_EmployeeTypes_RoleId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Employees_CreatedBy_Id",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Grades_Employees_CreatedBy_Id",
                table: "Grades");

            migrationBuilder.DropForeignKey(
                name: "FK_Pictures_Employees_CreatedBy_Id",
                table: "Pictures");

            migrationBuilder.DropForeignKey(
                name: "FK_RequestExits_Employees_CreatedBy_Id",
                table: "RequestExits");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentAbsents_Employees_CreatedBy_Id",
                table: "StudentAbsents");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentAbsents_Student_Class_Section_Years_StudentClassSectionYear_Student_Id_StudentClassSectionYear_Class_Id_StudentClassS~",
                table: "StudentAbsents");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentAttendances_AttendanceTypes_AttendanceTypeId",
                table: "StudentAttendances");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentAttendances_Employees_CreatedBy_Id",
                table: "StudentAttendances");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentAttendances_Student_Class_Section_Years_StudentClassSectionYear_Student_Id_StudentClassSectionYear_Class_Id_StudentCl~",
                table: "StudentAttendances");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentGrades_Working_Years_Working_Year_Id",
                table: "StudentGrades");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_Classes_ClassesId",
                table: "Students");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_Employees_CreatedBy_Id",
                table: "Students");

            migrationBuilder.DropTable(
                name: "Employee_Departments");

            migrationBuilder.DropTable(
                name: "Student_Class_Section_Years");

            migrationBuilder.DropTable(
                name: "Sections");

            migrationBuilder.DropTable(
                name: "Working_Years");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_StudentGrades_Working_Year_Id",
                table: "StudentGrades");

            migrationBuilder.DropIndex(
                name: "IX_StudentAttendances_AttendanceTypeId",
                table: "StudentAttendances");

            migrationBuilder.DropIndex(
                name: "IX_StudentAttendances_CreatedBy_Id",
                table: "StudentAttendances");

            migrationBuilder.DropIndex(
                name: "IX_StudentAttendances_StudentClassSectionYear_Student_Id_StudentClassSectionYear_Class_Id_StudentClassSectionYear_Working_Year_~",
                table: "StudentAttendances");

            migrationBuilder.DropIndex(
                name: "IX_StudentAbsents_CreatedBy_Id",
                table: "StudentAbsents");

            migrationBuilder.DropIndex(
                name: "IX_StudentAbsents_StudentClassSectionYear_Student_Id_StudentClassSectionYear_Class_Id_StudentClassSectionYear_Working_Year_Id_S~",
                table: "StudentAbsents");

            migrationBuilder.DropIndex(
                name: "IX_RequestExits_CreatedBy_Id",
                table: "RequestExits");

            migrationBuilder.DropIndex(
                name: "IX_Grades_CreatedBy_Id",
                table: "Grades");

            migrationBuilder.DropIndex(
                name: "IX_Employees_CreatedBy_Id",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Competences_CreatedBy_Id",
                table: "Competences");

            migrationBuilder.DropIndex(
                name: "IX_Competences_Department_Id",
                table: "Competences");

            migrationBuilder.DropIndex(
                name: "IX_Classes_CreatedBy_Id",
                table: "Classes");

            migrationBuilder.DropIndex(
                name: "IX_Classes_Department_Id",
                table: "Classes");

            migrationBuilder.DropIndex(
                name: "IX_AbsenceReasons_CreatedBy_Id",
                table: "AbsenceReasons");

            migrationBuilder.DropColumn(
                name: "Governate",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Working_Year_Id",
                table: "StudentGrades");

            migrationBuilder.DropColumn(
                name: "AttendanceTypeId",
                table: "StudentAttendances");

            migrationBuilder.DropColumn(
                name: "CreatedBy_Id",
                table: "StudentAttendances");

            migrationBuilder.DropColumn(
                name: "CustomReasonDetails",
                table: "StudentAttendances");

            migrationBuilder.DropColumn(
                name: "StudentClassSectionYear_Class_Id",
                table: "StudentAttendances");

            migrationBuilder.DropColumn(
                name: "StudentClassSectionYear_Section_id",
                table: "StudentAttendances");

            migrationBuilder.DropColumn(
                name: "StudentClassSectionYear_Student_Id",
                table: "StudentAttendances");

            migrationBuilder.DropColumn(
                name: "StudentClassSectionYear_Working_Year_Id",
                table: "StudentAttendances");

            migrationBuilder.DropColumn(
                name: "CreatedBy_Id",
                table: "StudentAbsents");

            migrationBuilder.DropColumn(
                name: "StudentClassSectionYear_Class_Id",
                table: "StudentAbsents");

            migrationBuilder.DropColumn(
                name: "StudentClassSectionYear_Section_id",
                table: "StudentAbsents");

            migrationBuilder.DropColumn(
                name: "StudentClassSectionYear_Student_Id",
                table: "StudentAbsents");

            migrationBuilder.DropColumn(
                name: "StudentClassSectionYear_Working_Year_Id",
                table: "StudentAbsents");

            migrationBuilder.DropColumn(
                name: "CreatedBy_Id",
                table: "Grades");

            migrationBuilder.DropColumn(
                name: "CreatedBy_Id",
                table: "EmployeeTypes");

            migrationBuilder.DropColumn(
                name: "CreatedBy_Id",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "CreatedBy_Id",
                table: "Competences");

            migrationBuilder.DropColumn(
                name: "Department_Id",
                table: "Competences");

            migrationBuilder.DropColumn(
                name: "CreatedBy_Id",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "Department_Id",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "CreatedBy_Id",
                table: "AbsenceReasons");

            migrationBuilder.RenameColumn(
                name: "CreatedBy_Id",
                table: "Students",
                newName: "ClassId");

            migrationBuilder.RenameColumn(
                name: "ClassesId",
                table: "Students",
                newName: "CreatedBy");

            migrationBuilder.RenameIndex(
                name: "IX_Students_CreatedBy_Id",
                table: "Students",
                newName: "IX_Students_ClassId");

            migrationBuilder.RenameIndex(
                name: "IX_Students_ClassesId",
                table: "Students",
                newName: "IX_Students_CreatedBy");

            migrationBuilder.RenameColumn(
                name: "CreatedBy_Id",
                table: "RequestExits",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "CreatedBy_Id",
                table: "Pictures",
                newName: "CreatedBy");

            migrationBuilder.RenameIndex(
                name: "IX_Pictures_CreatedBy_Id",
                table: "Pictures",
                newName: "IX_Pictures_CreatedBy");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "TaskEvaluations",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "StudentId",
                table: "TaskEvaluations",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "OutcomeId",
                table: "TaskEvaluations",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "birth_Certificate",
                table: "Students",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Picture_Profile",
                table: "Students",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Phone_Number_Mother",
                table: "Students",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Phone_Number_Father",
                table: "Students",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Phone_Number",
                table: "Students",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Natrual_Id",
                table: "Students",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Students",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Jop_of_Mother",
                table: "Students",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Jop_of_Father",
                table: "Students",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Students",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Date_of_birth",
                table: "Students",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Students",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Adress",
                table: "Students",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Governarate",
                table: "Students",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "StudentId",
                table: "StudentGrades",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "GradeId",
                table: "StudentGrades",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "StudentAttendances",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "StudentAttendances",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "StudentAttendances",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "StudentId",
                table: "StudentAttendances",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CustomReasonDetails",
                table: "StudentAbsents",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "AttendanceTypeId",
                table: "StudentAbsents",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "AbsenceReasonId",
                table: "StudentAbsents",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "AttendanceType",
                table: "StudentAbsents",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "StudentAbsents",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFieldAttendance",
                table: "StudentAbsents",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "StudentAbsents",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StudentGrade",
                table: "StudentAbsents",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StudentId",
                table: "StudentAbsents",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TeacherId",
                table: "StudentAbsents",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Reason",
                table: "RequestExits",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "AttendanceId",
                table: "RequestExits",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "ActualReturnTime",
                table: "RequestExits",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "RequestExits",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "ExpectedReturnTime",
                table: "RequestExits",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProcessedBy",
                table: "RequestExits",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ProcessedDate",
                table: "RequestExits",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProcessingNotes",
                table: "RequestExits",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "StudentId",
                table: "RequestExits",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TaskId",
                table: "Pictures",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "StudentId",
                table: "Pictures",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "FilePath",
                table: "Pictures",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Outcomes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "CompId",
                table: "Outcomes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Grades",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "Grades",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "EmployeeTypes",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "EmployeeTypes",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Employees",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "RoleId",
                table: "Employees",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Employees",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Employees",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastLogin",
                table: "Employees",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Join_Date",
                table: "Employees",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Employees",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "Employees",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Competences",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "Duration",
                table: "Competences",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "Competences",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FieldId",
                table: "Competences",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Classes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "Classes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FieldId",
                table: "Classes",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AttendanceTypes",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "AttendanceTypes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<bool>(
                name: "IsForFieldAttendance",
                table: "AttendanceTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AbsenceReasons",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "AbsenceReasons",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Fields",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    GradeId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
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
                name: "FieldEmployees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FieldId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true)
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

            migrationBuilder.CreateIndex(
                name: "IX_StudentAttendances_CreatedBy",
                table: "StudentAttendances",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAttendances_StudentId",
                table: "StudentAttendances",
                column: "StudentId");

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
                name: "IX_RequestExits_CreatedBy",
                table: "RequestExits",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RequestExits_ProcessedBy",
                table: "RequestExits",
                column: "ProcessedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RequestExits_StudentId",
                table: "RequestExits",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Grades_CreatedBy",
                table: "Grades",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeTypes_CreatedBy",
                table: "EmployeeTypes",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_CreatedBy",
                table: "Employees",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Competences_CreatedBy",
                table: "Competences",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Competences_FieldId",
                table: "Competences",
                column: "FieldId");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_CreatedBy",
                table: "Classes",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_FieldId",
                table: "Classes",
                column: "FieldId");

            migrationBuilder.CreateIndex(
                name: "IX_AbsenceReasons_CreatedBy",
                table: "AbsenceReasons",
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

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Employees_CreatedBy",
                table: "Employees",
                column: "CreatedBy",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeTypes_Employees_CreatedBy",
                table: "EmployeeTypes",
                column: "CreatedBy",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Grades_Employees_CreatedBy",
                table: "Grades",
                column: "CreatedBy",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Pictures_Employees_CreatedBy",
                table: "Pictures",
                column: "CreatedBy",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestExits_Employees_CreatedBy",
                table: "RequestExits",
                column: "CreatedBy",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestExits_Employees_ProcessedBy",
                table: "RequestExits",
                column: "ProcessedBy",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestExits_Students_StudentId",
                table: "RequestExits",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAbsents_Employees_CreatedBy",
                table: "StudentAbsents",
                column: "CreatedBy",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAbsents_Employees_TeacherId",
                table: "StudentAbsents",
                column: "TeacherId",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAbsents_Students_StudentId",
                table: "StudentAbsents",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAttendances_Employees_CreatedBy",
                table: "StudentAttendances",
                column: "CreatedBy",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAttendances_Students_StudentId",
                table: "StudentAttendances",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Classes_ClassId",
                table: "Students",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Employees_CreatedBy",
                table: "Students",
                column: "CreatedBy",
                principalTable: "Employees",
                principalColumn: "Id");
        }
    }
}
