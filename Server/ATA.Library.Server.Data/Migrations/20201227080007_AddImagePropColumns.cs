using Microsoft.EntityFrameworkCore.Migrations;

namespace ATA.Library.Server.Data.Migrations
{
    public partial class AddImagePropColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FileUrl",
                table: "Books",
                newName: "CoverImageFileFormat");

            migrationBuilder.AddColumn<string>(
                name: "BookFileFormat",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "BookFileSize",
                table: "Books",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "BookFileUrl",
                table: "Books",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookFileFormat",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "BookFileSize",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "BookFileUrl",
                table: "Books");

            migrationBuilder.RenameColumn(
                name: "CoverImageFileFormat",
                table: "Books",
                newName: "FileUrl");
        }
    }
}
