using System;
using System.Collections.Generic;

namespace PCME.MOPDB
{
    public partial class Money
    {
        public int Id { get; set; }
        public string PersonId { get; set; }
        public decimal MoneyVirtual { get; set; }
        public decimal MoneyActual { get; set; }
    }
}
