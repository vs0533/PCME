using System;
using System.Collections.Generic;

namespace PCME.MOPDB
{
    public partial class BmdUnitInfo
    {
        public int Id { get; set; }
        public string BmdName { get; set; }
        public string BmdAddress { get; set; }
        public int? PxdId { get; set; }
    }
}
