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

        [HttpGet("detail/edit")]
        public IActionResult GetDetailEdit()
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
                if (detail == null)
                {
                    return NotFound("Course doen't exist.");
                }
                else if (detail.UserId != userId)
                {
                    return Unauthorized("You don't have permission to do this.");
                }
                return Ok(detail);

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        [HttpGet("{userId}/detail")]
        public IActionResult GetFreeDetail(int userId)
        {
            try
            {
                var detail = _context.GetUserDetails(userId);
                return Ok(detail);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
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

        [HttpGet("nameemail")]
        public IActionResult GetNameEmail()
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

                var (name, email) = _context.GetUserNameAndEmail(userId);
                if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(email))
                {
                    return NotFound("User not found.");
                }
                return Ok(new { Name = name, Email = email });
            }
            catch (Exception)
            {
                return Unauthorized();
            }
        }

        [HttpGet("avatar")]
        public IActionResult GetAvatar()
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

                var name = _context.GetUserAvatar(userId);
                return Ok(name);
            }
            catch (Exception)
            {
                return Unauthorized();
            }
        }

        [HttpPut("updateFullName")]
        public IActionResult UpdateUserFullName([FromBody] UpdateUserFullNameDto dto)
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

                var user = _context.GetUserInfoByID(userId);
                if (user == null)
                {
                    return NotFound("User not found.");
                }

                else
                {
                    user.FullName = dto.FullName;
                    _context.UpdateUserInfo(user);
                }

                return Ok("User full name updated successfully.");
            }
            catch (Exception)
            {
                return Unauthorized();
            }

        }

        [HttpPut("updateEmail")]
        public IActionResult UpdateUserEmail([FromBody] UpdateUserEmailDto dto)
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

                var userInfo = _context.GetUserInfoByID(userId);
                if (userInfo == null)
                {
                    return NotFound("User not found.");
                }

                userInfo.Email = dto.Email;
                _context.UpdateUserInfo(userInfo);

                return Ok("User email updated successfully.");
            }
            catch (Exception)
            {
                return Unauthorized();
            }
        }

        [HttpPut("updatePhoneNumber")]
        public IActionResult UpdateUserPhoneNumber([FromBody] UpdateUserPhoneNumberDto dto)
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

                var userInfo = _context.GetUserInfoByID(userId);
                if (userInfo == null)
                {
                    return NotFound("User not found.");
                }

                userInfo.PhoneNumber = dto.PhoneNumber;
                _context.UpdateUserInfo(userInfo);

                return Ok("User phone number updated successfully.");
            }
            catch (Exception)
            {
                return Unauthorized();
            }
        }

        [HttpPut("updateDateOfBirth")]
        public IActionResult UpdateUserDateOfBirth([FromBody] UpdateUserDateOfBirthDto dto)
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

                var userInfo = _context.GetUserInfoByID(userId);
                if (userInfo == null)
                {
                    return NotFound("User not found.");
                }

                userInfo.DateOfBirth = dto.DateOfBirth;
                _context.UpdateUserInfo(userInfo);

                return Ok("User date of birth updated successfully.");
            }
            catch (Exception)
            {
                return Unauthorized();
            }
        }

        [HttpPut("updateGender")]
        public IActionResult UpdateUserGender([FromBody] UpdateUserGenderDto dto)
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

                var userInfo = _context.GetUserInfoByID(userId);
                if (userInfo == null)
                {
                    return NotFound("User not found.");
                }

                userInfo.Gender = dto.Gender;
                _context.UpdateUserInfo(userInfo);

                return Ok("User gender updated successfully.");
            }
            catch (Exception)
            {
                return Unauthorized();
            }
        }

        [HttpPut("updateAvatar")]
        public IActionResult UpdateUserAvatar([FromBody] UpdateUserAvatarDto dto)
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

                var userInfo = _context.GetUserInfoByID(userId);
                if (userInfo == null)
                {
                    return NotFound("User not found.");
                }

                userInfo.Avatar = dto.Avatar;
                _context.UpdateUserInfo(userInfo);

                return Ok("User avatar updated successfully.");
            }
            catch (Exception)
            {
                return Unauthorized();
            }
        }

        [HttpPut("updateAddress")]
        public IActionResult UpdateUserAddress([FromBody] UpdateUserAddressDto dto)
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

                var userInfo = _context.GetUserInfoByID(userId);
                if (userInfo == null)
                {
                    return NotFound("User not found.");
                }

                userInfo.UserAddress = dto.UserAddress;
                _context.UpdateUserInfo(userInfo);

                return Ok("User avatar updated successfully.");
            }
            catch (Exception)
            {
                return Unauthorized();
            }
        }

        [HttpPut("updateDescription")]
        public IActionResult UpdateUserAddress([FromBody] UpdateUserDescriptionDto dto)
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

                var userInfo = _context.GetUserInfoByID(userId);
                if (userInfo == null)
                {
                    return NotFound("User not found.");
                }

                userInfo.UserDescription = dto.UserDescription;
                _context.UpdateUserInfo(userInfo);

                return Ok("User avatar updated successfully.");
            }
            catch (Exception)
            {
                return Unauthorized();
            }
        }

        [HttpPut("updatePassword")]
        public IActionResult UpdateUserPassword([FromBody] UpdateUserPasswordDto dto)
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
                if (user == null)
                {
                    return NotFound("User not found.");
                }

                if (user.Password != dto.CurrentPassword) // Assuming passwords are stored in plain text, which is not recommended
                {
                    return BadRequest("Current password is incorrect.");
                }

                user.Password = dto.NewPassword;
                _context.UpdateUser(user);

                return Ok("User password updated successfully.");
            }
            catch (Exception)
            {
                return Unauthorized();
            }
        }

        [HttpPut("updateExpertise")]
        public IActionResult UpdateUserExpertise([FromBody] UpdateUserExpertiseDto dto)
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

                var userInfo = _context.GetUserInfoByID(userId);
                if (userInfo == null)
                {
                    return NotFound("User not found.");
                }

                userInfo.Expertise = dto.Expertise;
                _context.UpdateUserInfo(userInfo);

                return Ok("User expertise updated successfully.");
            }
            catch (Exception)
            {
                return Unauthorized();
            }
        }

        [HttpGet("teachers")]
        public IActionResult GetAllTeachers()
        {
            try
            {
                var teachers = _context.GetAllTeachers();
                if (teachers == null || !teachers.Any())
                {
                    return NotFound("No teachers found.");
                }
                return Ok(teachers);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet("users")]
        public IActionResult GetAllUsers()
        {
            try
            {
                var users = _context.GetUsers();
                if (users == null || !users.Any())
                {
                    return NotFound("No users found.");
                }
                return Ok(users);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}