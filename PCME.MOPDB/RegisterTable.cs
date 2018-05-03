using System;
using System.Collections.Generic;

namespace PCME.MOPDB
{
    public partial class RegisterTable
    {
        public int Id { get; set; }
        public string Num { get; set; }
        public string SubjectId { get; set; }
        public DateTime CreateTime { get; set; }
        public string UnitId { get; set; }
        public bool IsLocked { get; set; }
        public int CategoryTagValue { get; set; }
    }
}
