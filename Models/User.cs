using System;
using System.Collections.Generic;

namespace EduhubAPI.Models
{
    public partial class User
    {
        public User()
        {
            Courses = new HashSet<Course>();
            Enrollments = new HashSet<Enrollment>();
            Reviews = new HashSet<Review>();
            StudentLessons = new HashSet<StudentLesson>();
            StudentTestAttempts = new HashSet<StudentTestAttempt>();
            UserInfos = new HashSet<UserInfo>();
        }

        public int UserId { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int UserTypeId { get; set; }

        public virtual UserType UserType { get; set; } = null!;
        public virtual ICollection<Course> Courses { get; set; }
        public virtual ICollection<Enrollment> Enrollments { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<StudentLesson> StudentLessons { get; set; }
        public virtual ICollection<StudentTestAttempt> StudentTestAttempts { get; set; }
        public virtual ICollection<UserInfo> UserInfos { get; set; }
    }
}
