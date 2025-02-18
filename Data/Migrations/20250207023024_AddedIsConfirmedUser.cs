using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace reymani_web_api.Data.Migrations
{
  /// <inheritdoc />
  public partial class AddedIsConfirmedUser : Migration
  {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.AddColumn<bool>(
          name: "IsConfirmed",
          table: "Users",
          type: "boolean",
          nullable: false,
          defaultValue: false);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropColumn(
          name: "IsConfirmed",
          table: "Users");
    }
  }
}