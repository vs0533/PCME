using System;
using System.Collections.Generic;

namespace PCME.KSDB
{
    public partial class Test
    {
        public int Id { get; set; }
        public string Topic { get; set; }
        public string SelectItem { get; set; }
        public string Answer { get; set; }
        public string SubjectId { get; set; }
        public int? Type { get; set; }
    }
}
