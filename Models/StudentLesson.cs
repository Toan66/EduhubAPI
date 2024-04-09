using System;
using System.Collections.Generic;

namespace EduhubAPI.Models
{
    public partial class StudentLesson
    {
        public int UserId { get; set; }
        public int LessonId { get; set; }
        public DateTime CompleteDate { get; set; }

        public virtual Lesson Lesson { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
