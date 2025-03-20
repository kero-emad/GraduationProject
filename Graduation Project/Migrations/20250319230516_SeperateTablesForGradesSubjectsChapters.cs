using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Graduation_Project.Migrations
{
    /// <inheritdoc />
    public partial class SeperateTablesForGradesSubjectsChapters : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "subject",
                table: "teachers");

            migrationBuilder.DropColumn(
                name: "grade",
                table: "students");

            migrationBuilder.DropColumn(
                name: "chapter",
                table: "educationQuestions");

            migrationBuilder.DropColumn(
                name: "grade",
                table: "educationQuestions");

            migrationBuilder.DropColumn(
                name: "subject",
                table: "educationQuestions");

            migrationBuilder.AddColumn<int>(
                name: "SubjectId",
                table: "teachers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GradeId",
                table: "students",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ChapterId",
                table: "educationQuestions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GradeSubjectId",
                table: "educationQuestions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "grades",
                columns: table => new
                {
                    GradeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GradeName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_grades", x => x.GradeId);
                });

            migrationBuilder.CreateTable(
                name: "subjects",
                columns: table => new
                {
                    SubjectId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubjectName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_subjects", x => x.SubjectId);
                });

            migrationBuilder.CreateTable(
                name: "gradeSubject",
                columns: table => new
                {
                    GradeSubjectId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GradeId = table.Column<int>(type: "int", nullable: false),
                    SubjectId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_gradeSubject", x => x.GradeSubjectId);
                    table.ForeignKey(
                        name: "FK_gradeSubject_grades_GradeId",
                        column: x => x.GradeId,
                        principalTable: "grades",
                        principalColumn: "GradeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_gradeSubject_subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "subjects",
                        principalColumn: "SubjectId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "chapters",
                columns: table => new
                {
                    ChapterId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChapterNumber = table.Column<int>(type: "int", nullable: false),
                    ChapterName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GradeSubjectId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chapters", x => x.ChapterId);
                    table.ForeignKey(
                        name: "FK_chapters_gradeSubject_GradeSubjectId",
                        column: x => x.GradeSubjectId,
                        principalTable: "gradeSubject",
                        principalColumn: "GradeSubjectId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_teachers_SubjectId",
                table: "teachers",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_students_GradeId",
                table: "students",
                column: "GradeId");

            migrationBuilder.CreateIndex(
                name: "IX_educationQuestions_ChapterId",
                table: "educationQuestions",
                column: "ChapterId");

            migrationBuilder.CreateIndex(
                name: "IX_educationQuestions_GradeSubjectId",
                table: "educationQuestions",
                column: "GradeSubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_chapters_GradeSubjectId",
                table: "chapters",
                column: "GradeSubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_gradeSubject_GradeId",
                table: "gradeSubject",
                column: "GradeId");

            migrationBuilder.CreateIndex(
                name: "IX_gradeSubject_SubjectId",
                table: "gradeSubject",
                column: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_educationQuestions_chapters_ChapterId",
                table: "educationQuestions",
                column: "ChapterId",
                principalTable: "chapters",
                principalColumn: "ChapterId",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_educationQuestions_gradeSubject_GradeSubjectId",
                table: "educationQuestions",
                column: "GradeSubjectId",
                principalTable: "gradeSubject",
                principalColumn: "GradeSubjectId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_students_grades_GradeId",
                table: "students",
                column: "GradeId",
                principalTable: "grades",
                principalColumn: "GradeId",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_teachers_subjects_SubjectId",
                table: "teachers",
                column: "SubjectId",
                principalTable: "subjects",
                principalColumn: "SubjectId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_educationQuestions_chapters_ChapterId",
                table: "educationQuestions");

            migrationBuilder.DropForeignKey(
                name: "FK_educationQuestions_gradeSubject_GradeSubjectId",
                table: "educationQuestions");

            migrationBuilder.DropForeignKey(
                name: "FK_students_grades_GradeId",
                table: "students");

            migrationBuilder.DropForeignKey(
                name: "FK_teachers_subjects_SubjectId",
                table: "teachers");

            migrationBuilder.DropTable(
                name: "chapters");

            migrationBuilder.DropTable(
                name: "gradeSubject");

            migrationBuilder.DropTable(
                name: "grades");

            migrationBuilder.DropTable(
                name: "subjects");

            migrationBuilder.DropIndex(
                name: "IX_teachers_SubjectId",
                table: "teachers");

            migrationBuilder.DropIndex(
                name: "IX_students_GradeId",
                table: "students");

            migrationBuilder.DropIndex(
                name: "IX_educationQuestions_ChapterId",
                table: "educationQuestions");

            migrationBuilder.DropIndex(
                name: "IX_educationQuestions_GradeSubjectId",
                table: "educationQuestions");

            migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "teachers");

            migrationBuilder.DropColumn(
                name: "GradeId",
                table: "students");

            migrationBuilder.DropColumn(
                name: "ChapterId",
                table: "educationQuestions");

            migrationBuilder.DropColumn(
                name: "GradeSubjectId",
                table: "educationQuestions");

            migrationBuilder.AddColumn<string>(
                name: "subject",
                table: "teachers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "grade",
                table: "students",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "chapter",
                table: "educationQuestions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "grade",
                table: "educationQuestions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "subject",
                table: "educationQuestions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
