using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pochemychka.Migrations
{
    public partial class AddIdentity4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           
            migrationBuilder.DropIndex(
                name: "IX_Answers_IdQuestionNavigationIdQuestion",
                table: "Answers");

            migrationBuilder.DropColumn(
                name: "IdQuestionNavigationIdQuestion",
                table: "Answers");

            migrationBuilder.AddColumn<int>(
                name: "QuestionIdQuestion",
                table: "Answers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Answers_QuestionIdQuestion",
                table: "Answers",
                column: "QuestionIdQuestion");

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_Questions_QuestionIdQuestion",
                table: "Answers",
                column: "QuestionIdQuestion",
                principalTable: "Questions",
                principalColumn: "IdQuestion");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_Questions_QuestionIdQuestion",
                table: "Answers");

            migrationBuilder.DropIndex(
                name: "IX_Answers_QuestionIdQuestion",
                table: "Answers");

            migrationBuilder.DropColumn(
                name: "QuestionIdQuestion",
                table: "Answers");

            migrationBuilder.AddColumn<int>(
                name: "IdQuestionNavigationIdQuestion",
                table: "Answers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Answers_IdQuestionNavigationIdQuestion",
                table: "Answers",
                column: "IdQuestionNavigationIdQuestion");

        }
    }
}
