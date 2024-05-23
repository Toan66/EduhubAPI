namespace EduhubAPI.Dtos
{
    public class UserDetailsDto
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string UserType { get; set; }
        public UserInfoDto UserInfo { get; set; }
    }

    public class UserInfoDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string Avatar { get; set; }
        public string UserAddress { get; set; }
        public string UserDescription { get; set; }
        public string Expertise { get; set; }
    }
}
