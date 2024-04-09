using System;
using System.Collections.Generic;

namespace EduhubAPI.Models
{
    public partial class Test
    {
        public Test()
        {
            Questions = new HashSet<Question>();
            StudentTestAttempts = new HashSet<StudentTestAttempt>();
        }

        public int TestId { get; set; }
        public int ChapterId { get; set; }
        public string TestTitle { get; set; } = null!;
        public string TestDescription { get; set; } = null!;

        public virtual Chapter Chapter { get; set; } = null!;
        public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<StudentTestAttempt> StudentTestAttempts { get; set; }
    }
}
