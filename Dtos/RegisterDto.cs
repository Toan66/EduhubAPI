namespace EduhubAPI.Dtos
{
    public class RegisterDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public int UserTypeId { get; set; }
        // Add additional fields for UserInfo
        public string FullName { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string? Avatar { get; set; }

    }
}
