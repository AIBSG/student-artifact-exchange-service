using Artifact_Service_Api.AppData;
using Artifact_Service_Api.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using NpgsqlTypes;

namespace Artifact_Service_Api.Service
{
    public class FileService : IFileService
    {
        private readonly AppDbContext _context;

        public FileService(AppDbContext context)
        {
            _context = context;
        }

        private async Task<IEnumerable<DocumentNote>?> GetAllUserDocuments(Guid userId) =>
           await _context.DocumentNotes
            .Include(x => x.File)
            .Include(x => x.Author)
            .Include(x => x.DocumentNoteAccesses)
            .Include(x => x.DocumentNoteTags)
            .ThenInclude(x => x.Tag)
            .Where(x => x.Author.Id.Equals(userId))
            .ToArrayAsync();

        private async Task<IEnumerable<DocumentNote>?> GetAvailableDocuments(Guid userId) =>
            await _context.DocumentNoteAccesses
            .Include(x => x.User)
            .Include(x => x.DocumentNote)
            .Where(x => x.UserId.Equals(userId))
            .Select(x => x.DocumentNote)
            .Include(x => x.File)
            .Include(x => x.Author)
            .Include(x => x.DocumentNoteTags)
            .ThenInclude(x => x.Tag)
            .ToArrayAsync();

        public async Task<IEnumerable<Models.File?>> GetFilesByNote(Guid noteId) =>
            await _context.Notes
            .Where(x => x.Id.Equals(noteId))
            .Include(x => x.Files)
            .Select(x => x.Files)
            .FirstOrDefaultAsync();

        public IEnumerable<DocumentNote> GetAllDocuments(Guid userId) => 
            GetAllUserDocuments(userId).Result.Concat(GetAvailableDocuments(userId).Result);

        public IEnumerable<DocumentNote> GetAllDocumentsByTag(Guid userId, Guid tagId) => 
            GetAllDocuments(userId).Where(x => x.DocumentNoteTags.Select(x => x.Tag.Id).Contains(tagId));

        public async Task<IEnumerable<DocumentNote>?> GetAllOpenDocuments() => 
            await _context.DocumentNotes.Where(x => x.IsOpen).ToListAsync();
       
        public IEnumerable<DocumentNote>? GeOpenDocumentsByTag(Guid tagId) => 
            GetAllOpenDocuments().Result.Where(x => x.DocumentNoteTags.Select(x => x.Tag.Id).Contains(tagId));




    }
}
