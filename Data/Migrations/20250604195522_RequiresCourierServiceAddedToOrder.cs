using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace reymani_web_api.Data.Migrations
{
    /// <inheritdoc />
    public partial class RequiresCourierServiceAddedToOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_CourierId",
                table: "Orders");

            migrationBuilder.AlterColumn<int>(
                name: "CourierId",
                table: "Orders",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<bool>(
                name: "RequiresCourierService",
                table: "Orders",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Users_CourierId",
                table: "Orders",
                column: "CourierId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_CourierId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "RequiresCourierService",
                table: "Orders");

            migrationBuilder.AlterColumn<int>(
                name: "CourierId",
                table: "Orders",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Users_CourierId",
                table: "Orders",
                column: "CourierId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
