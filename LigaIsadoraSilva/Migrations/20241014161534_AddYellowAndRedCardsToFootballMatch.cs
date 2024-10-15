using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LigaIsadoraSilva.Migrations
{
    /// <inheritdoc />
    public partial class AddYellowAndRedCardsToFootballMatch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RedCards",
                table: "Games",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "YellowCards",
                table: "Games",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RedCards",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "YellowCards",
                table: "Games");
        }
    }
}
