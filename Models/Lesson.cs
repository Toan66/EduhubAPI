using System;
using System.Collections.Generic;

namespace EduhubAPI.Models
{
    public partial class Lesson
    {
        public Lesson()
        {
            StudentLessons = new HashSet<StudentLesson>();
        }

        public int LessonId { get; set; }
        public int ChapterId { get; set; }
        public string LessonTitle { get; set; } = null!;
        public string? LessonContent { get; set; }

        public virtual Chapter Chapter { get; set; } = null!;
        public virtual ICollection<StudentLesson> StudentLessons { get; set; }
    }
}
