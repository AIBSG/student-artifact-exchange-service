using Artifact_Service_Api.AppData;
using Artifact_Service_Api.Dtos;
using Artifact_Service_Api.Models;
using Microsoft.AspNetCore.Mvc;
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

        private async Task<IEnumerable<DocumentNote>?> GetUserDocuments(Guid userId) =>
           await _context.DocumentNotes
            .Include(x => x.File)
            .Include(x => x.Author)
            .Include(x => x.DocumentNoteAccesses)
            .ThenInclude(x => x.User)
            .Include(x => x.DocumentNoteTags)
            .ThenInclude(x => x.Tag)
            .Where(x => x.Author.Id.Equals(userId))
            .ToArrayAsync();

        private async Task<IEnumerable<DocumentNote>?> GetAvailableDocuments(Guid userId) =>
            await _context.DocumentNotes
            .Include(x => x.File)
            .Include(x => x.Author)
            .Include(x => x.DocumentNoteAccesses)
            .ThenInclude(x => x.User)
            .Include(x => x.DocumentNoteTags)
            .ThenInclude(x => x.Tag)
            .Where(x => x.DocumentNoteAccesses.Select(x => x.UserId).Contains(userId))
            .ToArrayAsync();

        public async Task<IEnumerable<Models.File?>> GetFilesByNote(Guid noteId) =>
            await _context.Notes
            .Where(x => x.Id.Equals(noteId))
            .Include(x => x.Files)
            .Select(x => x.Files)
            .FirstOrDefaultAsync();

        public IEnumerable<DocumentNote> GetAllUserDocuments(Guid userId) => 
            GetUserDocuments(userId).Result.Concat(GetAvailableDocuments(userId).Result);

        public IEnumerable<DocumentNote> GetAllUserDocumentsByTag(Guid userId, Guid tagId) => 
            GetAllUserDocuments(userId).Where(x => x.DocumentNoteTags.Select(x => x.Tag.Id).Contains(tagId));

        public async Task<IEnumerable<DocumentNote>?> GetAllOpenDocuments() => 
            await _context.DocumentNotes
            .Include(x => x.File)
            .Include(x => x.Author)
            .Include(x => x.DocumentNoteAccesses)
            .Include(x => x.DocumentNoteTags)
            .ThenInclude(x => x.Tag)
            .Where(x => x.IsOpen)
            .ToListAsync();
       
        public IEnumerable<DocumentNote>? GetOpenDocumentsByTag(Guid tagId) => 
            GetAllOpenDocuments().Result.Where(x => x.DocumentNoteTags.Select(x => x.Tag.Id).Contains(tagId));

        public async Task<DocumentNote> SaveNewDocument(NewDocumentRequest request, string serverFileName)
        {
            var result = new DocumentNote();
            result.Id = Guid.NewGuid();
            result.Author = await _context.Users.FirstOrDefaultAsync(x => x.Id == request.AuthorId);
            result.Title = request.Title;
            result.Description = request.Description;
            result.DocumentNoteAccesses = new List<DocumentNoteAccess>();
            result.IsOpen = request.IsOpen;
            result.DocumentNoteTags = new List<DocumentNoteTag>();
            var currentTags =  await _context.Tags.Where(x => request.TagsNames.Contains(x.Name)).ToListAsync();

            foreach ( var tag in currentTags)
            {
                result.DocumentNoteTags.Add(new DocumentNoteTag() { 
                    DocumentNote = result,
                    DocumentNoteId = result.Id,
                    Id = Guid.NewGuid(), Tag = tag, 
                    TagId = tag.Id });
            }

            var usersToAccess = await _context.Users.Where(x => request.MailsToAccess.Contains(x.Email)).ToArrayAsync();

            foreach (var user in usersToAccess)
            {
                result.DocumentNoteAccesses.Add(new DocumentNoteAccess()
                {
                    DocumentNoteId = result.Id,
                    UserId = user.Id,
                    Id = Guid.NewGuid(),
                    User = user,
                    DocumentNote = result
                });
            }

            

            result.File = new Models.File() { 
                Id = Guid.NewGuid(),
                CustomFileName = request.File.FileName,
                ServerFileName = serverFileName };


            await _context.DocumentNoteAccesses.AddRangeAsync(result.DocumentNoteAccesses);
            await _context.Set<DocumentNote>().AddAsync(result);
            await _context.SaveChangesAsync();

            return result;
        }

        public async Task<IEnumerable<string>?> ChangeDocumentAccess(IEnumerable<string> emails, Guid documentId)
        {
            var currentAccesses = await _context.DocumentNoteAccesses.Include(x => x.User).Where(x => x.DocumentNoteId.Equals(documentId)).ToListAsync();
            var currentUsers = currentAccesses.Select(x => x.User);
            var usersRequest = await _context.Users.Where(x => emails.Contains(x.Email)).ToListAsync();
            var document = await _context.DocumentNotes.SingleAsync(x => x.Id.Equals(documentId));
            if(currentAccesses != null)
            {
                var acceassesToKill = currentAccesses.Where(x => !emails.Contains(x.User.Email)).ToArray();
                if(acceassesToKill != null) _context.DocumentNoteAccesses.RemoveRange(acceassesToKill);
            }
           
            var usersToAdd = usersRequest.Where(x => !currentUsers.Contains(x)).ToList();
            if(usersToAdd != null)
            {
                var entitiesToAdd = new List<DocumentNoteAccess>();

                foreach (var user in usersToAdd)
                {
                    entitiesToAdd.Add(new DocumentNoteAccess()
                    {
                        Id = Guid.NewGuid(),
                        UserId = user.Id,
                        DocumentNoteId = documentId,
                        DocumentNote = document,
                        User = user
                    });
                }

                await _context.DocumentNoteAccesses.AddRangeAsync(entitiesToAdd);
            }
            await _context.SaveChangesAsync();
            return await _context.DocumentNoteAccesses.Where(x=>x.DocumentNoteId.Equals(documentId)).Select(x=>x.User.Email).ToListAsync();
        }
    }
}
