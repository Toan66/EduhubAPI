using System;
using System.Collections.Generic;

namespace EduhubAPI.Models
{
    public partial class Course
    {
        public Course()
        {
            Chapters = new HashSet<Chapter>();
            Enrollments = new HashSet<Enrollment>();
            Orders = new HashSet<Order>();
            Reviews = new HashSet<Review>();
        }

        public int CourseId { get; set; }
        public int TeacherId { get; set; }
        public string CourseName { get; set; } = null!;
        public string? CourseDescription { get; set; }
        public bool? ApprovalStatus { get; set; }
        public int CategoryId { get; set; }
        public string? FeatureImage { get; set; }
        public decimal? AverageRating { get; set; }
        public int? CoursePrice { get; set; }
        public int? CourseEarn { get; set; }
        public int? CourseLevelId { get; set; }

        public virtual CourseCategory Category { get; set; } = null!;
        public virtual CourseLevel? CourseLevel { get; set; }
        public virtual User Teacher { get; set; } = null!;
        public virtual ICollection<Chapter> Chapters { get; set; }
        public virtual ICollection<Enrollment> Enrollments { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
    }
}
