using System;
using System.Collections.Generic;

namespace EduhubAPI.Models
{
    public partial class CourseLevel
    {
        public CourseLevel()
        {
            Courses = new HashSet<Course>();
        }

        public int CourseLevelId { get; set; }
        public string CourseLevelName { get; set; } = null!;
        public string? CourseLevelDescription { get; set; }

        public virtual ICollection<Course> Courses { get; set; }
    }
}
