using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class edit_StudentClassSectionYear : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentAbsents_Student_Class_Section_Years_StudentClassSectionYear_Student_Id_StudentClassSectionYear_Class_Id_StudentClassS~",
                table: "StudentAbsents");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentAttendances_Student_Class_Section_Years_StudentClassSectionYear_Student_Id_StudentClassSectionYear_Class_Id_StudentCl~",
                table: "StudentAttendances");

            migrationBuilder.DropIndex(
                name: "IX_StudentAttendances_StudentClassSectionYear_Student_Id_StudentClassSectionYear_Class_Id_StudentClassSectionYear_Working_Year_~",
                table: "StudentAttendances");

            migrationBuilder.DropIndex(
                name: "IX_StudentAbsents_StudentClassSectionYear_Student_Id_StudentClassSectionYear_Class_Id_StudentClassSectionYear_Working_Year_Id_S~",
                table: "StudentAbsents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Student_Class_Section_Years",
                table: "Student_Class_Section_Years");

            migrationBuilder.RenameColumn(
                name: "StudentClassSectionYear_Class_Id",
                table: "StudentAttendances",
                newName: "Class_Id");

            migrationBuilder.RenameColumn(
                name: "StudentClassSectionYear_Class_Id",
                table: "StudentAbsents",
                newName: "Class_Id");

            migrationBuilder.AlterColumn<int>(
                name: "Section_id",
                table: "Student_Class_Section_Years",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Relational:ColumnOrder", 2)
                .OldAnnotation("Relational:ColumnOrder", 3);

            migrationBuilder.AlterColumn<int>(
                name: "Working_Year_Id",
                table: "Student_Class_Section_Years",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Relational:ColumnOrder", 1)
                .OldAnnotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<int>(
                name: "Class_Id",
                table: "Student_Class_Section_Years",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Student_Class_Section_Years",
                table: "Student_Class_Section_Years",
                columns: new[] { "Student_Id", "Working_Year_Id", "Section_id" });

            migrationBuilder.CreateIndex(
                name: "IX_StudentAttendances_Class_Id",
                table: "StudentAttendances",
                column: "Class_Id");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAttendances_StudentClassSectionYear_Student_Id_StudentClassSectionYear_Working_Year_Id_StudentClassSectionYear_Sectio~",
                table: "StudentAttendances",
                columns: new[] { "StudentClassSectionYear_Student_Id", "StudentClassSectionYear_Working_Year_Id", "StudentClassSectionYear_Section_id" });

            migrationBuilder.CreateIndex(
                name: "IX_StudentAbsents_Class_Id",
                table: "StudentAbsents",
                column: "Class_Id");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAbsents_StudentClassSectionYear_Student_Id_StudentClassSectionYear_Working_Year_Id_StudentClassSectionYear_Section_id",
                table: "StudentAbsents",
                columns: new[] { "StudentClassSectionYear_Student_Id", "StudentClassSectionYear_Working_Year_Id", "StudentClassSectionYear_Section_id" });

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAbsents_Classes_Class_Id",
                table: "StudentAbsents",
                column: "Class_Id",
                principalTable: "Classes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAbsents_Student_Class_Section_Years_StudentClassSectionYear_Student_Id_StudentClassSectionYear_Working_Year_Id_Studen~",
                table: "StudentAbsents",
                columns: new[] { "StudentClassSectionYear_Student_Id", "StudentClassSectionYear_Working_Year_Id", "StudentClassSectionYear_Section_id" },
                principalTable: "Student_Class_Section_Years",
                principalColumns: new[] { "Student_Id", "Working_Year_Id", "Section_id" });

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAttendances_Classes_Class_Id",
                table: "StudentAttendances",
                column: "Class_Id",
                principalTable: "Classes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAttendances_Student_Class_Section_Years_StudentClassSectionYear_Student_Id_StudentClassSectionYear_Working_Year_Id_St~",
                table: "StudentAttendances",
                columns: new[] { "StudentClassSectionYear_Student_Id", "StudentClassSectionYear_Working_Year_Id", "StudentClassSectionYear_Section_id" },
                principalTable: "Student_Class_Section_Years",
                principalColumns: new[] { "Student_Id", "Working_Year_Id", "Section_id" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentAbsents_Classes_Class_Id",
                table: "StudentAbsents");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentAbsents_Student_Class_Section_Years_StudentClassSectionYear_Student_Id_StudentClassSectionYear_Working_Year_Id_Studen~",
                table: "StudentAbsents");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentAttendances_Classes_Class_Id",
                table: "StudentAttendances");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentAttendances_Student_Class_Section_Years_StudentClassSectionYear_Student_Id_StudentClassSectionYear_Working_Year_Id_St~",
                table: "StudentAttendances");

            migrationBuilder.DropIndex(
                name: "IX_StudentAttendances_Class_Id",
                table: "StudentAttendances");

            migrationBuilder.DropIndex(
                name: "IX_StudentAttendances_StudentClassSectionYear_Student_Id_StudentClassSectionYear_Working_Year_Id_StudentClassSectionYear_Sectio~",
                table: "StudentAttendances");

            migrationBuilder.DropIndex(
                name: "IX_StudentAbsents_Class_Id",
                table: "StudentAbsents");

            migrationBuilder.DropIndex(
                name: "IX_StudentAbsents_StudentClassSectionYear_Student_Id_StudentClassSectionYear_Working_Year_Id_StudentClassSectionYear_Section_id",
                table: "StudentAbsents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Student_Class_Section_Years",
                table: "Student_Class_Section_Years");

            migrationBuilder.RenameColumn(
                name: "Class_Id",
                table: "StudentAttendances",
                newName: "StudentClassSectionYear_Class_Id");

            migrationBuilder.RenameColumn(
                name: "Class_Id",
                table: "StudentAbsents",
                newName: "StudentClassSectionYear_Class_Id");

            migrationBuilder.AlterColumn<int>(
                name: "Class_Id",
                table: "Student_Class_Section_Years",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<int>(
                name: "Section_id",
                table: "Student_Class_Section_Years",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Relational:ColumnOrder", 3)
                .OldAnnotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<int>(
                name: "Working_Year_Id",
                table: "Student_Class_Section_Years",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Relational:ColumnOrder", 2)
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Student_Class_Section_Years",
                table: "Student_Class_Section_Years",
                columns: new[] { "Student_Id", "Class_Id", "Working_Year_Id", "Section_id" });

            migrationBuilder.CreateIndex(
                name: "IX_StudentAttendances_StudentClassSectionYear_Student_Id_StudentClassSectionYear_Class_Id_StudentClassSectionYear_Working_Year_~",
                table: "StudentAttendances",
                columns: new[] { "StudentClassSectionYear_Student_Id", "StudentClassSectionYear_Class_Id", "StudentClassSectionYear_Working_Year_Id", "StudentClassSectionYear_Section_id" });

            migrationBuilder.CreateIndex(
                name: "IX_StudentAbsents_StudentClassSectionYear_Student_Id_StudentClassSectionYear_Class_Id_StudentClassSectionYear_Working_Year_Id_S~",
                table: "StudentAbsents",
                columns: new[] { "StudentClassSectionYear_Student_Id", "StudentClassSectionYear_Class_Id", "StudentClassSectionYear_Working_Year_Id", "StudentClassSectionYear_Section_id" });

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAbsents_Student_Class_Section_Years_StudentClassSectionYear_Student_Id_StudentClassSectionYear_Class_Id_StudentClassS~",
                table: "StudentAbsents",
                columns: new[] { "StudentClassSectionYear_Student_Id", "StudentClassSectionYear_Class_Id", "StudentClassSectionYear_Working_Year_Id", "StudentClassSectionYear_Section_id" },
                principalTable: "Student_Class_Section_Years",
                principalColumns: new[] { "Student_Id", "Class_Id", "Working_Year_Id", "Section_id" });

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAttendances_Student_Class_Section_Years_StudentClassSectionYear_Student_Id_StudentClassSectionYear_Class_Id_StudentCl~",
                table: "StudentAttendances",
                columns: new[] { "StudentClassSectionYear_Student_Id", "StudentClassSectionYear_Class_Id", "StudentClassSectionYear_Working_Year_Id", "StudentClassSectionYear_Section_id" },
                principalTable: "Student_Class_Section_Years",
                principalColumns: new[] { "Student_Id", "Class_Id", "Working_Year_Id", "Section_id" });
        }
    }
}
