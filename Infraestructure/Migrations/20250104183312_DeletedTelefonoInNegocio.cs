using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace reymani_web_api.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class DeletedTelefonoInNegocio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Telefono",
                table: "Negocios");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Telefono",
                table: "Negocios",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
