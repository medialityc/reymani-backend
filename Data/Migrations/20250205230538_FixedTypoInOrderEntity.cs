using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace reymani_web_api.Data.Migrations
{
  /// <inheritdoc />
  public partial class FixedTypoInOrderEntity : Migration
  {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropColumn(
          name: "CourierUserId",
          table: "Orders");

      migrationBuilder.DropColumn(
          name: "CustomerUserId",
          table: "Orders");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.AddColumn<int>(
          name: "CourierUserId",
          table: "Orders",
          type: "integer",
          nullable: true);

      migrationBuilder.AddColumn<int>(
          name: "CustomerUserId",
          table: "Orders",
          type: "integer",
          nullable: false,
          defaultValue: 0);
    }
  }
}