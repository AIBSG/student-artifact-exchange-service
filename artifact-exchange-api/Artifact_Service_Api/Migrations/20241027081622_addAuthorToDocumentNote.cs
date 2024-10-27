using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Artifact_Service_Api.Migrations
{
    /// <inheritdoc />
    public partial class addAuthorToDocumentNote : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AuthorId",
                table: "DocumentNotes",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_DocumentNotes_AuthorId",
                table: "DocumentNotes",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentNotes_Users_AuthorId",
                table: "DocumentNotes",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentNotes_Users_AuthorId",
                table: "DocumentNotes");

            migrationBuilder.DropIndex(
                name: "IX_DocumentNotes_AuthorId",
                table: "DocumentNotes");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "DocumentNotes");
        }
    }
}
