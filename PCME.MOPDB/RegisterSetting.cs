using System;
using System.Collections.Generic;

namespace PCME.MOPDB
{
    public partial class RegisterSetting
    {
        public int Id { get; set; }
        public short Year { get; set; }
        public string SubjectId { get; set; }
        public bool IsActive { get; set; }
        public short CategoryTagValue { get; set; }
    }
}
