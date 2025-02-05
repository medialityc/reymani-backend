using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace reymani_web_api.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixedTypoInCustomerAddressId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_UserAddresses_UserAddressId",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "UserAddressId",
                table: "Orders",
                newName: "CustomerAddressId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_UserAddressId",
                table: "Orders",
                newName: "IX_Orders_CustomerAddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_UserAddresses_CustomerAddressId",
                table: "Orders",
                column: "CustomerAddressId",
                principalTable: "UserAddresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_UserAddresses_CustomerAddressId",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "CustomerAddressId",
                table: "Orders",
                newName: "UserAddressId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_CustomerAddressId",
                table: "Orders",
                newName: "IX_Orders_UserAddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_UserAddresses_UserAddressId",
                table: "Orders",
                column: "UserAddressId",
                principalTable: "UserAddresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
