using System;

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace reymani_web_api.Data.Migrations
{
  /// <inheritdoc />
  public partial class AddedHerencyToBaseEntity : Migration
  {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropForeignKey(
          name: "FK_Orders_Users_CourierId",
          table: "Orders");

      migrationBuilder.AlterColumn<short>(
          name: "Role",
          table: "Users",
          type: "smallint",
          nullable: false,
          oldClrType: typeof(int),
          oldType: "integer");

      migrationBuilder.AddColumn<DateTimeOffset>(
          name: "CreatedAt",
          table: "ShoppingCartItems",
          type: "timestamp with time zone",
          nullable: false,
          defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

      migrationBuilder.AddColumn<DateTimeOffset>(
          name: "UpdatedAt",
          table: "ShoppingCartItems",
          type: "timestamp with time zone",
          nullable: false,
          defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

      migrationBuilder.AlterColumn<short>(
          name: "Status",
          table: "Orders",
          type: "smallint",
          nullable: false,
          oldClrType: typeof(int),
          oldType: "integer");

      migrationBuilder.AlterColumn<short>(
          name: "PaymentMethod",
          table: "Orders",
          type: "smallint",
          nullable: false,
          oldClrType: typeof(int),
          oldType: "integer");

      migrationBuilder.AlterColumn<int>(
          name: "CourierId",
          table: "Orders",
          type: "integer",
          nullable: true,
          oldClrType: typeof(int),
          oldType: "integer");

      migrationBuilder.AddColumn<DateTimeOffset>(
          name: "CreatedAt",
          table: "OrderItems",
          type: "timestamp with time zone",
          nullable: false,
          defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

      migrationBuilder.AddColumn<DateTimeOffset>(
          name: "UpdatedAt",
          table: "OrderItems",
          type: "timestamp with time zone",
          nullable: false,
          defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

      migrationBuilder.AddColumn<DateTimeOffset>(
          name: "UpdatedAt",
          table: "ConfirmationNumbers",
          type: "timestamp with time zone",
          nullable: false,
          defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

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
          name: "CreatedAt",
          table: "ShoppingCartItems");

      migrationBuilder.DropColumn(
          name: "UpdatedAt",
          table: "ShoppingCartItems");

      migrationBuilder.DropColumn(
          name: "CreatedAt",
          table: "OrderItems");

      migrationBuilder.DropColumn(
          name: "UpdatedAt",
          table: "OrderItems");

      migrationBuilder.DropColumn(
          name: "UpdatedAt",
          table: "ConfirmationNumbers");

      migrationBuilder.AlterColumn<int>(
          name: "Role",
          table: "Users",
          type: "integer",
          nullable: false,
          oldClrType: typeof(short),
          oldType: "smallint");

      migrationBuilder.AlterColumn<int>(
          name: "Status",
          table: "Orders",
          type: "integer",
          nullable: false,
          oldClrType: typeof(short),
          oldType: "smallint");

      migrationBuilder.AlterColumn<int>(
          name: "PaymentMethod",
          table: "Orders",
          type: "integer",
          nullable: false,
          oldClrType: typeof(short),
          oldType: "smallint");

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