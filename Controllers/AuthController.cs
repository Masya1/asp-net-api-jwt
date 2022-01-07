using Auth.Dtos;
using Auth.Models;
using Auth.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Linq;

namespace Auth.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserContext _userContext;
        private readonly IPasswordHasher _passwordHasher;
        private readonly JwtService _jwtService;

        public AuthController(UserContext userContext,
         IPasswordHasher passwordHasher,
         JwtService jwtService)
        {
            this._userContext = userContext;
            this._passwordHasher = passwordHasher;
            this._jwtService = jwtService;
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto regDto)
        {
            User user = new User
            {
                Name = regDto.Name,
                Password = _passwordHasher.MakeHash(regDto.Password),
                Email = regDto.Email
            };
            _userContext.Users.Add(user);
            user.Id = await _userContext.SaveChangesAsync();
            return Created("api/auth/register", user);
        }

        [HttpGet]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("x-token");
            return Ok();
        }

        public IActionResult Login(LoginDto loginDto)
        {
            User userToCheck = _userContext.Users.FirstOrDefault(u => u.Email == loginDto.Email);

            if (userToCheck == null ||
                !_passwordHasher.Validate(
                    userToCheck.Password,
                    loginDto.Password))
            {
                return BadRequest(new { Message = "Invalid credentials." });
            }

            string token = _jwtService.Generate(userToCheck.Id);
            Response.Cookies.Append("x-token", token, new CookieOptions()
            {
                HttpOnly = true
            });
            return Ok(new { Message = "Successful login!" });
        }
    }
}
