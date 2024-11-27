using Artifact_Service_Api.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Artifact_Service_Api.Service
{
    public interface IJwtService
    {
        public string GenerateToken(User user);
    }
}
