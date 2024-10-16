using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LigaIsadoraSilva.Migrations
{
    /// <inheritdoc />
    public partial class StaffPhotofixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Staffs_AspNetUsers_UserId",
                table: "Staffs");

            migrationBuilder.DropForeignKey(
                name: "FK_Staffs_StaffDuties_StaffDutiyId",
                table: "Staffs");

            migrationBuilder.DropIndex(
                name: "IX_Staffs_StaffDutiyId",
                table: "Staffs");

            migrationBuilder.DropIndex(
                name: "IX_Staffs_UserId",
                table: "Staffs");

            migrationBuilder.DropColumn(
                name: "StaffDutiyId",
                table: "Staffs");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Staffs");

            migrationBuilder.AlterColumn<int>(
                name: "StaffDutyId",
                table: "Staffs",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Staffs_StaffDutyId",
                table: "Staffs",
                column: "StaffDutyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Staffs_StaffDuties_StaffDutyId",
                table: "Staffs",
                column: "StaffDutyId",
                principalTable: "StaffDuties",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Staffs_StaffDuties_StaffDutyId",
                table: "Staffs");

            migrationBuilder.DropIndex(
                name: "IX_Staffs_StaffDutyId",
                table: "Staffs");

            migrationBuilder.AlterColumn<string>(
                name: "StaffDutyId",
                table: "Staffs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StaffDutiyId",
                table: "Staffs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Staffs",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Staffs_StaffDutiyId",
                table: "Staffs",
                column: "StaffDutiyId");

            migrationBuilder.CreateIndex(
                name: "IX_Staffs_UserId",
                table: "Staffs",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Staffs_AspNetUsers_UserId",
                table: "Staffs",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Staffs_StaffDuties_StaffDutiyId",
                table: "Staffs",
                column: "StaffDutiyId",
                principalTable: "StaffDuties",
                principalColumn: "Id");
        }
    }
}
