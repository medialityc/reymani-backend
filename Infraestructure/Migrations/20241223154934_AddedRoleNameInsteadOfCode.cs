using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace reymani_web_api.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedRoleNameInsteadOfCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Codigo",
                table: "Roles",
                newName: "Nombre");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Nombre",
                table: "Roles",
                newName: "Codigo");
        }
    }
}
