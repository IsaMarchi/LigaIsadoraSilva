using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LigaIsadoraSilva.Migrations
{
    /// <inheritdoc />
    public partial class AddcorrectIsFinalizedToFootballMatch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFinalized",
                table: "Clubs");

            migrationBuilder.AddColumn<bool>(
                name: "IsFinalized",
                table: "Games",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFinalized",
                table: "Games");

            migrationBuilder.AddColumn<bool>(
                name: "IsFinalized",
                table: "Clubs",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
