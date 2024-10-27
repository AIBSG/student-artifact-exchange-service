namespace Artifact_Service_Api.Service
{
    public interface IFileStorageService
    {
        public Task<byte[]>? GetFileBites(Models.File file);
        public IEnumerable<Task<byte[]>>? GetFilesBitesByNote(IEnumerable<Models.File> files);
        public string SaveNewFileInStorage(IFormFile file);
    }
}
