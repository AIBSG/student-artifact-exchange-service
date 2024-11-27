using Artifact_Service_Api.AppData;
using Artifact_Service_Api.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Artifact_Service_Api.Service
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IJwtService _jwtService;

        public UserService(AppDbContext context, IJwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        public async Task Register(string login, string password)
        {
           await _context.Users.AddAsync(new Models.User
           {
               Id = Guid.NewGuid(), 
               Email = login,
               Password = password,
               ActivatedEmail = false,
               GAcoount = false,
               RegistryCode = 1111,
           });

           await _context.SaveChangesAsync();
        }

        public async Task<string> Login(string login, string password)
        {
            var user = await _context.Users
                .Where(x => x.Email == login && x.Password == password)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new KeyNotFoundException();
            }

            var token = _jwtService.GenerateToken(user);

            return token;
        }
    }
}
