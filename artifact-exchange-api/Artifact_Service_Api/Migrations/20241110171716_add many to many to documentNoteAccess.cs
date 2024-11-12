using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Artifact_Service_Api.Migrations
{
    /// <inheritdoc />
    public partial class addmanytomanytodocumentNoteAccess : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FilePath",
                table: "Files");

            migrationBuilder.AlterColumn<int>(
                name: "RegistryCode",
                table: "Users",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "RegistryCode",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                table: "Files",
                type: "text",
                nullable: true);
        }
    }
}
