using System;
using System.Collections.Generic;

namespace PCME.MOPDB
{
    public partial class UnitApply
    {
        public string ApplyId { get; set; }
        public string UnitId { get; set; }
        public string SubjectId { get; set; }
        public decimal Amount { get; set; }
        public short ApplyStateValue { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
