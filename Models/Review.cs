using System;
using System.Collections.Generic;

namespace EduhubAPI.Models
{
    public partial class Review
    {
        public int ReviewId { get; set; }
        public int? CourseId { get; set; }
        public int? UserId { get; set; }
        public decimal? Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime? ReviewDate { get; set; }
        public virtual Course? Course { get; set; }
        public virtual User? User { get; set; }
    }
}
