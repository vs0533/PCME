using System;
using System.Collections.Generic;

namespace PCME.MOPDB
{
    public partial class Account
    {
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public string UnitId { get; set; }
        public int TypeId { get; set; }
        public string Password { get; set; }

        public Unit Unit { get; set; }
    }
}
