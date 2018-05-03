using System;
using System.Collections.Generic;

namespace PCME.MOPDB
{
    public partial class Kdaccount
    {
        public int Id { get; set; }
        public string LogName { get; set; }
        public string LogPassWord { get; set; }
        public int UnitId { get; set; }
        public int RoleId { get; set; }
    }
}
