using System;
using System.Collections.Generic;

namespace EduhubAPI.Models
{
    public partial class StudentTestAttempt
    {
        public int TestAttemptId { get; set; }
        public int TestId { get; set; }
        public int UserId { get; set; }
        public DateTime AttemptDate { get; set; }
        public decimal? Score { get; set; }

        public virtual Test Test { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
