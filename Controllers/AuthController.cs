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
                UserTypeId = dto.UserTypeId,
                UserInfos = new List<UserInfo>
                {
                    new UserInfo
                    {
                        FullName = dto.FullName,
                        Email = dto.Email,
                        DateOfBirth = dto.DateOfBirth,
                        Gender = dto.Gender,
                        PhoneNumber = dto.PhoneNumber
                    }
                }
            };

            return Created("success", _context.Create(user));
        }

        [HttpPost("login")]
        public IActionResult Login(LoginDto dto)
        {
            var user = _context.GetByUserName(dto.Username);

            if (user == null) return BadRequest(new { message = "Username don't exist!" });

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
            {
                return BadRequest(new { message = "Wrong password" });
            }

            // Retrieve the user's role
            var role = _context.GetUserRole(user.UserId);

            // Assuming roles are managed, passing the user's role for roles.
            var jwt = _jwtService.Generate(user.UserId, new List<string> { role });


            Response.Cookies.Append("jwt", jwt, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None
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

        [HttpGet("name")]
        public IActionResult GetName()
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

                var name = _context.GetUserName(userId);
                return Ok(name);
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

        //[HttpGet]
        //public IActionResult GetUsers()
        //{
        //    var users = _context.GetAllUsers();
        //    return Ok(users);
        //}

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
            // Đảm bảo rằng các tùy chọn cookie khi xóa giống với khi bạn tạo cookie
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // Sử dụng Secure = true nếu cookie được tạo với tùy chọn này
                SameSite = SameSiteMode.None // Sử dụng SameSite = SameSiteMode.None nếu cookie được tạo với tùy chọn này
            };

            Response.Cookies.Delete("jwt", cookieOptions);

            return Ok(new
            {
                message = "success"
            });
        }
    }
}
