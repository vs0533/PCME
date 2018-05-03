using System;
using System.Collections.Generic;

namespace PCME.MOPDB
{
    public partial class ExamSubjectZwClass
    {
        public int Id { get; set; }
        public string ClassId { get; set; }
        public string SubjectId { get; set; }
        public bool? Isenable { get; set; }
    }
}
