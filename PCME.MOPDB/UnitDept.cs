using System;
using System.Collections.Generic;

namespace PCME.MOPDB
{
    public partial class UnitDept
    {
        public int DeptId { get; set; }
        public string DeptName { get; set; }
        public string UnitId { get; set; }

        public Unit Unit { get; set; }
    }
}
