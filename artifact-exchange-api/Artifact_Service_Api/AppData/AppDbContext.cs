using Microsoft.EntityFrameworkCore;
using Artifact_Service_Api.Models;

namespace Artifact_Service_Api.AppData;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {}

    public DbSet<User> Users { get; set; }
    public DbSet<Note> Notes { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<NoteAccess> NoteAccesses { get; set; }
    public DbSet<NoteTag> NoteTags { get; set; }
    public DbSet<Models.File> Files { get; set; }
    public DbSet<DocumentNote> DocumentNotes { get; set; }
    public DbSet<DocumentNoteTag> DocumentNoteTags { get; set; }
    public DbSet<DocumentNoteAccess> DocumentNoteAccesses { get; set; }
}