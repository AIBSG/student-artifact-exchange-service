using Artifact_Service_Api.Dtos;
using Artifact_Service_Api.Service;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Artifact_Service_Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            await _userService.Register(request.Email, request.Password);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var token = await _userService.Login(email, password);

            HttpContext.Response.Cookies.Append("cokes", token);
            return Ok(token); 
        }
    }
}
