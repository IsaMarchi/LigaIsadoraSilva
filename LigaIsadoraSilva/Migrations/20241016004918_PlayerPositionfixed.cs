using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LigaIsadoraSilva.Migrations
{
    /// <inheritdoc />
    public partial class PlayerPositionfixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_PayersPositions_PlayersPositionId",
                table: "Players");

            migrationBuilder.DropTable(
                name: "PayersPositions");

            migrationBuilder.DropIndex(
                name: "IX_Players_PlayersPositionId",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "PlayersPositionId",
                table: "Players");

            migrationBuilder.AddColumn<string>(
                name: "Position",
                table: "Players",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Position",
                table: "Players");

            migrationBuilder.AddColumn<int>(
                name: "PlayersPositionId",
                table: "Players",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PayersPositions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayersPositions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Players_PlayersPositionId",
                table: "Players",
                column: "PlayersPositionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Players_PayersPositions_PlayersPositionId",
                table: "Players",
                column: "PlayersPositionId",
                principalTable: "PayersPositions",
                principalColumn: "Id");
        }
    }
}
