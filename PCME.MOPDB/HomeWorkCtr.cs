using System;
using System.Collections.Generic;

namespace PCME.MOPDB
{
    public partial class HomeWorkCtr
    {
        public int Id { get; set; }
        public string PersonId { get; set; }
        public int HomeWorkCtr1 { get; set; }
        public string SubjectId { get; set; }
        public DateTime? OverDateTime { get; set; }
        public string WorkState { get; set; }
    }
}
