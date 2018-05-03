using System;
using System.Collections.Generic;

namespace PCME.MOPDB
{
    public partial class LevelChange
    {
        public int ChangeId { get; set; }
        public string PersonId { get; set; }
        public int TechDuty { get; set; }
        public DateTime ChangeDate { get; set; }
    }
}
