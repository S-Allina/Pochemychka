using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pochemychka.Migrations
{
    public partial class AddIdentity5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
