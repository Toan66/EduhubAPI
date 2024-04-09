using EduhubAPI.Helpers;
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
        public UserController(UserRepository context)
        {
            _context = context;
        }
        [HttpGet("DetailsFromCookie")]
        public IActionResult GetUserDetailsFromCookie()
        {
            // Đọc ID người dùng từ cookies
            if (HttpContext.Request.Cookies.TryGetValue("userId", out string userIdValue) && int.TryParse(userIdValue, out int userId))
            {
                var userDetails = _context.GetUserDetails(userId);
                if (userDetails == null)
                {
                    return NotFound();
                }

                return Ok(userDetails);
            }
            else
            {
                return BadRequest("User ID not found.");
            }
        }
    }
}
