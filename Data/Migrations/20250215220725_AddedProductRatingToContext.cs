using System;

using Microsoft.EntityFrameworkCore.Migrations;

using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace reymani_web_api.Data.Migrations
{
  /// <inheritdoc />
  public partial class AddedProductRatingToContext : Migration
  {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.CreateTable(
          name: "ProductRatings",
          columns: table => new
          {
            Id = table.Column<int>(type: "integer", nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            ProductId = table.Column<int>(type: "integer", nullable: false),
            UserId = table.Column<int>(type: "integer", nullable: false),
            Rating = table.Column<int>(type: "integer", nullable: false),
            CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
            UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_ProductRatings", x => x.Id);
            table.ForeignKey(
                      name: "FK_ProductRatings_Products_ProductId",
                      column: x => x.ProductId,
                      principalTable: "Products",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
            table.ForeignKey(
                      name: "FK_ProductRatings_Users_UserId",
                      column: x => x.UserId,
                      principalTable: "Users",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateIndex(
          name: "IX_ProductRatings_ProductId",
          table: "ProductRatings",
          column: "ProductId");

      migrationBuilder.CreateIndex(
          name: "IX_ProductRatings_UserId",
          table: "ProductRatings",
          column: "UserId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(
          name: "ProductRatings");
    }
  }
}