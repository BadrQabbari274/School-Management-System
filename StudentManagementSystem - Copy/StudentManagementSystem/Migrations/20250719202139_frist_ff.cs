using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class frist_ff : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "StudentAttendances",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "CustomReasonDetails",
                table: "StudentAbsents",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsFieldAttendance",
                table: "StudentAbsents",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "ActualReturnTime",
                table: "RequestExits",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "ExitTime",
                table: "RequestExits",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

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
                name: "Status",
                table: "RequestExits",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StudentId",
                table: "RequestExits",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaxStudents",
                table: "Classes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "AttendanceTypes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsForFieldAttendance",
                table: "AttendanceTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_RequestExits_ProcessedBy",
                table: "RequestExits",
                column: "ProcessedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RequestExits_StudentId",
                table: "RequestExits",
                column: "StudentId");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestExits_Employees_ProcessedBy",
                table: "RequestExits");

            migrationBuilder.DropForeignKey(
                name: "FK_RequestExits_Students_StudentId",
                table: "RequestExits");

            migrationBuilder.DropIndex(
                name: "IX_RequestExits_ProcessedBy",
                table: "RequestExits");

            migrationBuilder.DropIndex(
                name: "IX_RequestExits_StudentId",
                table: "RequestExits");

            migrationBuilder.DropColumn(
                name: "CustomReasonDetails",
                table: "StudentAbsents");

            migrationBuilder.DropColumn(
                name: "IsFieldAttendance",
                table: "StudentAbsents");

            migrationBuilder.DropColumn(
                name: "ActualReturnTime",
                table: "RequestExits");

            migrationBuilder.DropColumn(
                name: "ExitTime",
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
                name: "Status",
                table: "RequestExits");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "RequestExits");

            migrationBuilder.DropColumn(
                name: "MaxStudents",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "AttendanceTypes");

            migrationBuilder.DropColumn(
                name: "IsForFieldAttendance",
                table: "AttendanceTypes");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "StudentAttendances",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
