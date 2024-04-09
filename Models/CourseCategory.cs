using System;
using System.Collections.Generic;

namespace EduhubAPI.Models
{
    public partial class CourseCategory
    {
        public CourseCategory()
        {
            Courses = new HashSet<Course>();
        }

        public int CourseCategoryId { get; set; }
        public string CourseCategoryName { get; set; } = null!;

        public virtual ICollection<Course> Courses { get; set; }
    }
}
