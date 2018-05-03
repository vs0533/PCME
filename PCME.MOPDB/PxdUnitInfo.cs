using System;
using System.Collections.Generic;

namespace PCME.MOPDB
{
    public partial class PxdUnitInfo
    {
        public int Id { get; set; }
        public string PxdName { get; set; }
        public string PxdAddress { get; set; }
        public string PxdPhone { get; set; }
        public string PxdLinkMan { get; set; }
        public string PxdEmail { get; set; }
        public string PxdClass { get; set; }
        public int? Type { get; set; }
    }
}
