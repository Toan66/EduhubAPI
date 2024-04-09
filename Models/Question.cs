using System;
using System.Collections.Generic;

namespace EduhubAPI.Models
{
    public partial class Question
    {
        public Question()
        {
            Answers = new HashSet<Answer>();
        }

        public int QuestionId { get; set; }
        public int TestId { get; set; }
        public string QuestionContent { get; set; } = null!;

        public virtual Test Test { get; set; } = null!;
        public virtual ICollection<Answer> Answers { get; set; }
    }
}
