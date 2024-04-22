using System;
using System.Collections.Generic;

namespace EduhubAPI.Models
{
    public partial class UserCurrency
    {
        public int UserCurrencyId { get; set; }
        public int UserId { get; set; }
        public int CurrencyId { get; set; }
        public int? Balance { get; set; }

        public virtual Currency Currency { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
