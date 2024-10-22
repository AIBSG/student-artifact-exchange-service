using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Artifact_Service_Api.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tags_DocumentNotes_DocumentNoteId",
                table: "Tags");

            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Notes_NoteId",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Tags_DocumentNoteId",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Tags_NoteId",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "DocumentNoteId",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "NoteId",
                table: "Tags");

            migrationBuilder.CreateTable(
                name: "DocumentNoteTags",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DocumentNoteId = table.Column<Guid>(type: "uuid", nullable: false),
                    TagId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentNoteTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentNoteTags_DocumentNotes_DocumentNoteId",
                        column: x => x.DocumentNoteId,
                        principalTable: "DocumentNotes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DocumentNoteTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NoteTags",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    NoteId = table.Column<Guid>(type: "uuid", nullable: false),
                    TagId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoteTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NoteTags_Notes_NoteId",
                        column: x => x.NoteId,
                        principalTable: "Notes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NoteTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentNoteTags_DocumentNoteId",
                table: "DocumentNoteTags",
                column: "DocumentNoteId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentNoteTags_TagId",
                table: "DocumentNoteTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_NoteTags_NoteId",
                table: "NoteTags",
                column: "NoteId");

            migrationBuilder.CreateIndex(
                name: "IX_NoteTags_TagId",
                table: "NoteTags",
                column: "TagId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocumentNoteTags");

            migrationBuilder.DropTable(
                name: "NoteTags");

            migrationBuilder.AddColumn<Guid>(
                name: "DocumentNoteId",
                table: "Tags",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "NoteId",
                table: "Tags",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tags_DocumentNoteId",
                table: "Tags",
                column: "DocumentNoteId");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_NoteId",
                table: "Tags",
                column: "NoteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_DocumentNotes_DocumentNoteId",
                table: "Tags",
                column: "DocumentNoteId",
                principalTable: "DocumentNotes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Notes_NoteId",
                table: "Tags",
                column: "NoteId",
                principalTable: "Notes",
                principalColumn: "Id");
        }
    }
}
