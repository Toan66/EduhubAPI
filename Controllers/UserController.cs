using EduhubAPI.Dtos;
using EduhubAPI.Helpers;
using EduhubAPI.Models;
using EduhubAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace EduhubAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserRepository _context;
        private readonly JwtService _jwtService;

        public UserController(UserRepository context, JwtService jwt)
        {
            _context = context;
            _jwtService = jwt;
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

        [HttpPut("updateFullName/{userId}")]
        public IActionResult UpdateUserFullName(int userId, [FromBody] UpdateUserFullNameDto dto)
        {
            var user = _context.GetUserByID(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Assuming you have a method in your repository to update the user's full name
            _context.UpdateUserFullName(userId, dto.FullName);
            return Ok("User full name updated successfully.");
        }

        [HttpPut("updatePhoneNumber/{userId}")]
        public IActionResult UpdateUserPhoneNumber(int userId, [FromBody] UpdateUserPhoneNumberDto dto)
        {
            var user = _context.GetUserByID(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            _context.UpdateUserPhoneNumber(userId, dto.PhoneNumber);
            return Ok("User phone number updated successfully.");
        }

        [HttpPut("updateDateOfBirth/{userId}")]
        public IActionResult UpdateUserDateOfBirth(int userId, [FromBody] UpdateUserDateOfBirthDto dto)
        {
            var user = _context.GetUserByID(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            _context.UpdateUserDateOfBirth(userId, dto.DateOfBirth);
            return Ok("User date of birth updated successfully.");
        }

        [HttpPut("updateGender/{userId}")]
        public IActionResult UpdateUserGender(int userId, [FromBody] UpdateUserGenderDto dto)
        {
            var user = _context.GetUserByID(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            _context.UpdateUserGender(userId, dto.Gender);
            return Ok("User gender updated successfully.");
        }

        [HttpPut("updateEmail/{userId}")]
        public IActionResult UpdateUserEmail(int userId, [FromBody] UpdateUserEmailDto dto)
        {
            var user = _context.GetUserByID(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            _context.UpdateUserEmail(userId, dto.Email);
            return Ok("User email updated successfully.");
        }
        [HttpPut("updateUserInfo/{userId}")]
        public IActionResult UpdateUserInfo(int userId, [FromBody] UpdateUserInfoDto dto)
        {
            var user = _context.GetUserByID(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            _context.UpdateUserInfo(userId, dto);
            return Ok("User information updated successfully.");
        }
        [HttpPut("updateAvatar/{userId}")]
        public IActionResult UpdateUserAvatar(int userId, [FromBody] UpdateUserAvatarDto dto)
        {
            var user = _context.GetUserByID(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }
        
            _context.UpdateUserAvatar(userId, dto.Avatar);
            return Ok("User avatar updated successfully.");
        }
    }
}
