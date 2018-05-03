using System;
using System.Collections.Generic;

namespace PCME.MOPDB
{
    public partial class ExamScore
    {
        public int Id { get; set; }
        public string PersonId { get; set; }
        public int Examscore { get; set; }
        public string SubjectId { get; set; }
    }
}
