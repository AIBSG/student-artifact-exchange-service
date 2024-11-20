namespace Artifact_Service_Api.Service
{
    public interface IFileStorageService
    {
        public Task<byte[]>? GetFileBites(string serverFileName);
        public IEnumerable<Task<byte[]>>? GetFilesBitesByNote(string[] serverFileNames);
        public string SaveNewFileInStorage(IFormFile file);
    }
}
