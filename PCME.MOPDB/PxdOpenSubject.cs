using System;
using System.Collections.Generic;

namespace PCME.MOPDB
{
    public partial class PxdOpenSubject
    {
        public int Id { get; set; }
        public int PxdId { get; set; }
        public string SubjectId { get; set; }
        public bool? Isenable { get; set; }
        public bool? Is2 { get; set; }
    }
}
