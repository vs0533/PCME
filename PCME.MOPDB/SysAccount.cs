using System;
using System.Collections.Generic;

namespace PCME.MOPDB
{
    public partial class SysAccount
    {
        public int Id { get; set; }
        public string SysLogName { get; set; }
        public string SysLogPass { get; set; }
        public int RoleId { get; set; }
    }
}
