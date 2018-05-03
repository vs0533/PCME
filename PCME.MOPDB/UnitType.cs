using System;
using System.Collections.Generic;

namespace PCME.MOPDB
{
    public partial class UnitType
    {
        public UnitType()
        {
            Unit = new HashSet<Unit>();
        }

        public int Id { get; set; }
        public string TypeName { get; set; }

        public ICollection<Unit> Unit { get; set; }
    }
}
