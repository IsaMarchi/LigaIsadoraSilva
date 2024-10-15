using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LigaIsadoraSilva.Migrations
{
    /// <inheritdoc />
    public partial class FixedUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FootballTeamId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_FootballTeamId",
                table: "AspNetUsers",
                column: "FootballTeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Clubs_FootballTeamId",
                table: "AspNetUsers",
                column: "FootballTeamId",
                principalTable: "Clubs",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Clubs_FootballTeamId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_FootballTeamId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FootballTeamId",
                table: "AspNetUsers");
        }
    }
}
