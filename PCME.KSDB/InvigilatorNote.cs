using System;
using System.Collections.Generic;

namespace PCME.KSDB
{
    public partial class InvigilatorNote
    {
        public int Id { get; set; }
        public string Ccxh { get; set; }
        public string Remark { get; set; }
        public string InvigilatorTeacher { get; set; }
        public DateTime Ksdate { get; set; }
        public DateTime SaveTime { get; set; }
        public int Skrs { get; set; }
        public int Wjrs { get; set; }
        public int Qkrs { get; set; }
    }
}
