using System;
using System.Collections.Generic;

namespace PCME.MOPDB
{
    public partial class Print
    {
        public string PersonId { get; set; }
        public string Idcard { get; set; }
        public string PrintOrder { get; set; }
        public string PrintState { get; set; }
        public DateTime? Selecttime { get; set; }
        public DateTime? Printtime { get; set; }
    }
}
