using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Artifact_Service_Api.Migrations
{
    /// <inheritdoc />
    public partial class addOpenNotes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsOpen",
                table: "Notes",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsOpen",
                table: "DocumentNotes",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOpen",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "IsOpen",
                table: "DocumentNotes");
        }
    }
}
