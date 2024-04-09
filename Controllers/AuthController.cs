using EduhubAPI.Dtos;
using EduhubAPI.Helpers;
using EduhubAPI.Models;
using EduhubAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EduhubAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserRepository _context;
        private readonly JwtService _jwtService;
        public AuthController(UserRepository context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }
        [HttpPost("register")]
        public IActionResult Register(RegisterDto dto)
        {
            var user = new User
            {
                Username = dto.Username,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                UserTypeId = dto.UserTypeId
            };

            return Created("success", _context.Create(user));
        }

        [HttpPost("login")]
        public IActionResult Login(LoginDto dto)
        {
            var user = _context.GetByUserName(dto.Username);

            if (user == null) return BadRequest(new { message = "Invalid Credentials" });

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
            {
                return BadRequest(new { message = "Invalid Credentials" });
            }

            // Retrieve the user's role
            var role = _context.GetUserRole(user.UserId);

            // Assuming roles are managed, passing the user's role for roles.
            var jwt = _jwtService.Generate(user.UserId, new List<string> { role });

            Response.Cookies.Append("jwt", jwt, new CookieOptions
            {
                HttpOnly = true
            });

            return Ok(new
            {
                message = "success",
                token = jwt,
                role = role
            });
        }

        [HttpGet("role")]
        public IActionResult GetRole()
        {
            try
            {
                var jwt = Request.Cookies["jwt"];
                if (string.IsNullOrEmpty(jwt))
                {
                    return Unauthorized();
                }
                var token = _jwtService.Verify(jwt);
                int userId = int.Parse(token.Issuer);

                var role = _context.GetUserRole(userId);
                return Ok(role);
            }
            catch (Exception)
            {
                return Unauthorized();
            }
        }

        [HttpGet("detail")]
        public IActionResult GetDetail()
        {
            try
            {
                var jwt = Request.Cookies["jwt"];
                if (string.IsNullOrEmpty(jwt))
                {
                    return Unauthorized();
                }
                var token = _jwtService.Verify(jwt);
                int userId = int.Parse(token.Issuer);

                var detail = _context.GetUserDetails(userId);
                return Ok(detail);
            }
            catch (Exception)
            {
                return Unauthorized();
            }
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            var users = _context.GetAllUsers();
            return Ok(users);
        }

        [HttpGet("user")]
        public IActionResult SingleUser()
        {
            try
            {
                var jwt = Request.Cookies["jwt"];
                if (string.IsNullOrEmpty(jwt))
                {
                    return Unauthorized();
                }
                var token = _jwtService.Verify(jwt);

                int userId = int.Parse(token.Issuer);

                var user = _context.GetUserByID(userId);

                return Ok(user);
            }
            catch (Exception)
            {
                return Unauthorized();
            }
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt");

            return Ok(new
            {
                message = "success"
            });
        }
    }
}
