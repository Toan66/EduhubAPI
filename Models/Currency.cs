using System;
using System.Collections.Generic;

namespace EduhubAPI.Models
{
    public partial class Currency
    {
        public Currency()
        {
            UserCurrencies = new HashSet<UserCurrency>();
        }

        public int CurrencyId { get; set; }
        public string CurrencyName { get; set; } = null!;
        public string CurrencyCode { get; set; } = null!;

        public virtual ICollection<UserCurrency> UserCurrencies { get; set; }
    }
}
