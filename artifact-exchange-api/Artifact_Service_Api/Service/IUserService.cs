using Artifact_Service_Api.AppData;

namespace Artifact_Service_Api.Service
{
    public interface IUserService
    {
        public Task Register(string login, string password);

        public Task<string> Login(string login, string password);
    }
}

