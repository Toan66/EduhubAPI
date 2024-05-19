using System;
using System.Collections.Generic;

namespace EduhubAPI.Models
{
    public partial class StudentChapter
    {
        public int StudentChapterId { get; set; }
        public int UserId { get; set; }
        public int ChapterId { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsUnlocked { get; set; }
        public DateTime? CompletionDate { get; set; }
        public decimal? CompletePercent { get; set; }

        public virtual Chapter Chapter { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
