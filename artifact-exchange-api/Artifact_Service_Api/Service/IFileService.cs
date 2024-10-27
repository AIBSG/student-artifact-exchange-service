using Artifact_Service_Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Artifact_Service_Api.Service
{
    public interface IFileService
    {
        public Task<IEnumerable<Models.File?>> GetFilesByNote(Guid noteId);
        public IEnumerable<DocumentNote> GetAllDocuments(Guid userId);
        public IEnumerable<DocumentNote> GetAllDocumentsByTag(Guid userId, Guid tagId);
        public Task<IEnumerable<DocumentNote>?> GetAllOpenDocuments();
        public IEnumerable<DocumentNote>? GeOpenDocumentsByTag(Guid tagId);
    }
}
