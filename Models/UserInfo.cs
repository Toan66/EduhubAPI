using System;
using System.Collections.Generic;

namespace EduhubAPI.Models
{
    public partial class UserInfo
    {
        public int UserInfoId { get; set; }
        public int UserId { get; set; }
        public string FullName { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Avatar { get; set; }
        public string? UserDescription { get; set; }
        public string? UserAddress { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
