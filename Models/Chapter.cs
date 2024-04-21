using System;
using System.Collections.Generic;

namespace EduhubAPI.Models
{
    public partial class Chapter
    {
        public Chapter()
        {
            Lessons = new HashSet<Lesson>();
            Tests = new HashSet<Test>();
        }

        public int ChapterId { get; set; }
        public int CourseId { get; set; }
        public string ChapterTitle { get; set; } = null!;
        public string? ChapterDescription { get; set; }
        public int? ChapterOrder { get; set; }

        public virtual Course Course { get; set; } = null!;
        public virtual ICollection<Lesson> Lessons { get; set; }
        public virtual ICollection<Test> Tests { get; set; }
    }
}
