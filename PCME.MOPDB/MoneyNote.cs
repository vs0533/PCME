using System;
using System.Collections.Generic;

namespace PCME.MOPDB
{
    public partial class MoneyNote
    {
        public int Id { get; set; }
        public string AdminId { get; set; }
        public string PersonId { get; set; }
        public decimal Money { get; set; }
        public string MacId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime IsOverDate { get; set; }
        public string OverAddress { get; set; }
        public bool IsPrint { get; set; }
        public int? Type { get; set; }
    }
}
