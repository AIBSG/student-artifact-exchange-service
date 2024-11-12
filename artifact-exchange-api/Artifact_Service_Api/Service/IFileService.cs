using Artifact_Service_Api.Dtos;
using Artifact_Service_Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Artifact_Service_Api.Service
{
    public interface IFileService
    {
        public Task<IEnumerable<Models.File?>> GetFilesByNote(Guid noteId);
        public IEnumerable<DocumentNote> GetAllUserDocuments(Guid userId);
        public IEnumerable<DocumentNote> GetAllUserDocumentsByTag(Guid userId, Guid tagId);
        public Task<IEnumerable<DocumentNote>?> GetAllOpenDocuments();
        public IEnumerable<DocumentNote>? GetOpenDocumentsByTag(Guid tagId);
        public Task<DocumentNote> SaveNewDocument(NewDocumentRequest request, string serverFileName);

        public  Task<IEnumerable<string>?> ChangeDocumentAccess(IEnumerable<string> emails, Guid documentId);
    }
}