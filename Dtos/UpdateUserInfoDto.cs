namespace EduhubAPI.Dtos
{
    public class UpdateUserInfoDto
    {
        public string FullName { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; } = null!;
        public string Email { get; set; } = null!;
    }

    public class UpdateUserFullNameDto
    {
        public string FullName { get; set; }
    }

    public class UpdateUserPhoneNumberDto
    {
        public string PhoneNumber { get; set; }
    }

    public class UpdateUserDateOfBirthDto
    {
        public DateTime DateOfBirth { get; set; }
    }

    public class UpdateUserGenderDto
    {
        public string Gender { get; set; }
    }

    public class UpdateUserEmailDto
    {
        public string Email { get; set; }
    }
    public class UpdateUserAvatarDto
    {
        public string Avatar { get; set; }
    }

}
