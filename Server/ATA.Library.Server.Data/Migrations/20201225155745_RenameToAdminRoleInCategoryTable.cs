using Microsoft.EntityFrameworkCore.Migrations;

namespace ATA.Library.Server.Data.Migrations
{
    public partial class RenameToAdminRoleInCategoryTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AccessRole",
                table: "Categories",
                newName: "AdminRole");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AdminRole",
                table: "Categories",
                newName: "AccessRole");
        }
    }
}
